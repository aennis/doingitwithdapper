using ContactsDirectory.Models;
using Infrastructure.Objects;
using Infrastructure.Objects.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using Services;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ContactsDirectory.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        List<SelectListItem> _firmsList = new List<SelectListItem>();
        List<SelectListItem> _sourcesLst = new List<SelectListItem>();
        List<SelectListItem> _locationsLst = new List<SelectListItem>();
        List<SelectListItem> _practiceAreaLst = new List<SelectListItem>();
        List<SelectListItem> _pqeLst = new List<SelectListItem>();


        public readonly IAppService _appsService;
        private readonly IToastNotification _toastNotification;
        private readonly IHostingEnvironment _env;
        public HomeController(
               IAppService appsService
             , IHostingEnvironment env
             , IToastNotification toastNotification
            
           )
        {
            _env = env;
            _appsService = appsService;
            _toastNotification = toastNotification;
          
        }


        #region Gets
        public async Task<IActionResult> Index(int view_option = 1, string search="", int page = 1, int pageSize = 6)
        {

            try
            {


                AzureUser.CurrentUser = User.Identity.Name;


            }
            catch
            {
                AzureUser.CurrentUser = "N/A";
            }

            PagedResults<ContactsListViewModel> contacts ;

            //Search Param needs to be kept when paging on search results
            if (search != null && search.Length > 0)
            {
                //do search
                if (search.ToLower() == "all")
                {
                    contacts = await _appsService.GetUserContacts(AzureUser.CurrentUser, page, pageSize);
                    //reset search param
                    search = "";
                    if (contacts == null || contacts.TotalCount == 0)
                    {
                        ViewBag.Title = "No Contacts Found";
                    }
                }
                else
                {
            
                    //This search method needs to be added as PagedResult
                    contacts = await _appsService.SearchUserContacts(AzureUser.CurrentUser, search, page, pageSize);
                    if (contacts == null || contacts.TotalCount== 0)
                    {
                        ViewBag.Title = "No Contacts Found in Search";
                        ViewBag.SubTitle = "Type 'all' into Search to Return to All Contacts";
                    }
                    else
                    {
                        ViewBag.Title = "Your Contacts";
                        ViewBag.SubTitle = "Type 'all' into Search to Return to All Contacts";
                    }
                }

            }
            else
            {
                //get all
                contacts = await _appsService.GetUserContacts(AzureUser.CurrentUser, page, pageSize);
                search = "";
                if (contacts == null || contacts.TotalCount == 0) 
                { 
                    ViewBag.Title = "No Contacts Found";
                }
                else
                {
                    ViewBag.Title = "Your Contacts";
                }
            }

            ViewBag.UserAd = AzureUser.CurrentUser;
            ViewData["View_Option"] = view_option;
            ViewBag.SearchInput = search;
            return View(contacts);
        }
     


        /// <summary>
        /// New Contact View
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> New()
        {
            ContactViewModel viewModel = new ContactViewModel();

            await LoadLists();
            ViewBag.FirmsList = _firmsList;
            ViewBag.SourceList = _sourcesLst;
            ViewBag.LocationList = _locationsLst;
            ViewBag.PracticeAreaList = _practiceAreaLst;
            ViewBag.PQEsList = _pqeLst;


            return View(viewModel);
        }

        /// <summary>
        /// Loads a Contact
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(int id)
        {
            ContactDto  contact= await _appsService.GetContact(id);
            ContactViewModel viewModel = new ContactViewModel {
                 Id = contact.Id
                ,ContactName = contact.ContactName
                ,MatterNo = contact.MatterNo
                ,Location =contact.Location
                ,PQE = contact.PQE
                ,PracticeArea = contact.PracticeArea
                ,Firm = contact.Firm
                ,Source = contact.Source
                ,SourceOther = contact.SourceOther
                ,CreatedOn = contact.CreatedOn
                ,CreatedBy = contact.CreatedBy
            };

            await LoadLists();
            ViewBag.FirmsList = _firmsList;
            ViewBag.SourceList = _sourcesLst;
            ViewBag.LocationList = _locationsLst;
            ViewBag.PracticeAreaList = _practiceAreaLst;
            ViewBag.PQEsList = _pqeLst;
            return View(viewModel);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        #endregion



        #region Posts



        /// <summary>
        /// Add new Contact
        /// </summary>
        /// <param name="form"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> New([Bind("ContactName,MatterNo,Location,PracticeArea,Firm,PQE,CreatedOn,Source,SourceOther")] ContactViewModel form, string returnUrl = null)
        {

            // var AzureUser.CurrentUser = _httpContextAccessor.HttpContext.User.Identity.Name.Split('\\')[1];
            form.CreatedBy = AzureUser.CurrentUser; //TODO: set to cached current user
            if (ModelState.IsValid)
            {

                int exists = await _appsService.CheckIfContactExists(form.ContactName, form.Firm);
                if (exists > 0)
                {
                    _toastNotification.AddErrorToastMessage("Contact Already Exists", new ToastrOptions()
                    {
                        ProgressBar = false
                    });

                    await LoadLists();
                    ViewBag.FirmsList = _firmsList;
                    ViewBag.SourceList = _sourcesLst;
                    ViewBag.LocationList = _locationsLst;
                    ViewBag.PracticeAreaList = _practiceAreaLst;
                    ViewBag.PQEsList = _pqeLst;
                    return View(form);
                }


                //MAPPER NEEDED
                ContactDto entry = new ContactDto
                {
                    ContactName = form.ContactName
                 ,  MatterNo = form.MatterNo
                 ,  Location = form.Location
                 ,  PQE = form.PQE
                 ,  PracticeArea = form.PracticeArea
                 ,  Firm = form.Firm
                 ,  CreatedOn = form.CreatedOn
                 ,  CreatedBy = form.CreatedBy
                 ,  Source = form.Source
                 ,  SourceOther = form.SourceOther
                };
                                                       
                //
                int result = await _appsService.CreateContact(entry);
                if (result >= 1)
                {
                    //trying to return to were we came from (Could use category to determine this also)
                    _toastNotification.AddInfoToastMessage("New Contact Added", new ToastrOptions()
                    {
                        ProgressBar = false
                    });
                    return RedirectToAction("Index");
                }
                else
                {
                    _toastNotification.AddErrorToastMessage("Error, Contact Not Added", new ToastrOptions()
                    {
                        ProgressBar = false
                    });
                }

            }
            else
            {
                await LoadLists();
                ViewBag.FirmsList = _firmsList;
                ViewBag.SourceList = _sourcesLst;
                ViewBag.LocationList = _locationsLst;
                ViewBag.PracticeAreaList = _practiceAreaLst;
                ViewBag.PQEsList = _pqeLst;
                return View(form);
            }

            await LoadLists();
            ViewBag.FirmsList = _firmsList;
            ViewBag.SourceList = _sourcesLst;
            ViewBag.LocationList = _locationsLst;
            ViewBag.PracticeAreaList = _practiceAreaLst;
            ViewBag.PQEsList = _pqeLst;
            return View(form);

        }



        /// <summary>
        /// Update Contact
        /// </summary>
        /// <param name="form"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,ContactName,MatterNo,Location,PracticeArea,Firm,PQE,Source,SourceOther")] ContactViewModel form, string returnUrl = null)
        {
           
        
            form.Modifiedby = AzureUser.CurrentUser; //TODO: set to cached current user
            if (ModelState.IsValid)
            {
                //MAPPER NEEDED
                ContactDto entry = new ContactDto
                {

                    ContactName = form.ContactName
                 ,
                    MatterNo = form.MatterNo
                 ,
                    Location = form.Location
                 ,
                    PQE = form.PQE
                 ,
                    PracticeArea = form.PracticeArea
                 ,
                    Firm = form.Firm
                 ,
                    Source = form.Source
                 ,
                    Modifiedby = form.Modifiedby
                 ,
                    Id = form.Id
                };

                //
                int result = await _appsService.UpdateContact(entry);
                if (result >= 1)
                {
                    //trying to return to were we came from (Could use category to determine this also)
                    _toastNotification.AddInfoToastMessage("Contact Saved", new ToastrOptions()
                    {
                        ProgressBar = false
                    });
                    return RedirectToAction("Index");
                }
                else
                {
                    _toastNotification.AddErrorToastMessage("Error, Contact Not Saved", new ToastrOptions()
                    {
                        ProgressBar = false
                    });
                }

            }
            else
            {

                await LoadLists();
                ViewBag.FirmsList = _firmsList;
                ViewBag.SourceList = _sourcesLst;
                ViewBag.LocationList = _locationsLst;
                ViewBag.PracticeAreaList = _practiceAreaLst;
                ViewBag.PQEsList = _pqeLst;
                return View(form);
            }


            await LoadLists();
            ViewBag.FirmsList = _firmsList;
            ViewBag.SourceList = _sourcesLst;
            ViewBag.LocationList = _locationsLst;
            ViewBag.PracticeAreaList = _practiceAreaLst;
            ViewBag.PQEsList = _pqeLst;
            return View(form);
        }


        public async Task<ActionResult> Delete(int id)
        {

            //
            int result = await _appsService.DeleteContact(id, AzureUser.CurrentUser);
                if (result >= 1)
                {
                    //trying to return to were we came from (Could use category to determine this also)
                    _toastNotification.AddInfoToastMessage("Contact Deleted", new ToastrOptions()
                    {
                        ProgressBar = false
                    });
                }
                else
                {
                    _toastNotification.AddErrorToastMessage("Error, Contact Not Deleted", new ToastrOptions()
                    {
                        ProgressBar = false
                    });
                }


            return RedirectToAction("Index");
        }


        #endregion




        /// <summary>
        /// Load Drop Lists  (Could be Cached)
        /// </summary>
        /// <returns></returns>
        public async Task LoadLists()
        {
            IEnumerable<FirmDto> FirmLst = new List<FirmDto>();
            IEnumerable<SourceDto> SourceLst = new List<SourceDto>();
            IEnumerable<LocationDto> LocationLst = new List<LocationDto>();
            IEnumerable<PracticeAreaDto> PracticeAreaLst = new List<PracticeAreaDto>();
            IEnumerable<PQEDto> PQEsLst = new List<PQEDto>();

            FirmLst = await _appsService.GetFirms();
            foreach (FirmDto pr in FirmLst)
            {
                var selectListItem = new SelectListItem
                {
                    Value = pr.Id.ToString(), //Value of the object, should be unique
                    Text = pr.Firm //Text that will be displayed
                };

                _firmsList.Add(selectListItem);
            }

            SourceLst = await _appsService.GetSources();
            foreach (SourceDto st in SourceLst)
            {
                var selectListItem = new SelectListItem
                {
                    Value = st.Id.ToString(), //Value of the object, should be unique
                    Text = st.Source //Text that will be displayed
                };

                _sourcesLst.Add(selectListItem);
            }

            LocationLst = await _appsService.GetLocations();
            foreach (LocationDto st in LocationLst)
            {
                var selectListItem = new SelectListItem
                {
                    Value = st.Id.ToString(), //Value of the object, should be unique
                    Text = st.Location //Text that will be displayed
                };

                _locationsLst.Add(selectListItem);
            }

            PracticeAreaLst = await _appsService.GetPracticeAreas();
            foreach (PracticeAreaDto st in PracticeAreaLst)
            {
                var selectListItem = new SelectListItem
                {
                    Value = st.Id.ToString(), //Value of the object, should be unique
                    Text = st.PracticeArea //Text that will be displayed
                };

                _practiceAreaLst.Add(selectListItem);
            }

            PQEsLst = await _appsService.GetPQEs();
            foreach (PQEDto st in PQEsLst)
            {
                var selectListItem = new SelectListItem
                {
                    Value = st.Id.ToString(), //Value of the object, should be unique
                    Text = st.PQE //Text that will be displayed
                };

                _pqeLst.Add(selectListItem);
            }


        }

    }
}

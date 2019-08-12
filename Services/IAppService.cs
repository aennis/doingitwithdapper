using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Dapper;
using Infrastructure.Objects;
using Infrastructure.Objects.Dtos;
using Infrastructure.Repository.DbConnection;
using Infrastructure.Repository.SqlServer;
using ContactsDirectory.Models;

namespace Services
{
    public interface IAppService
    {
        //Gets
        Task<PagedResults<ContactsListViewModel>> GetUserContacts(string user, int page = 1, int pageSize = 5);
        Task<ContactDto> GetContact(int id);
        Task<int> CheckIfContactExists(string person, int firm);

        //Modify
        Task<int> CreateContact(ContactDto entry);
        Task<int> UpdateContact(ContactDto entry);
        Task<int> DeleteContact(int Id, string user);

        Task<PagedResults<ContactsListViewModel>> SearchUserContacts(string user, string param, int page = 1, int pageSize = 6);

        //lookups
        Task<IEnumerable<SourceDto>> GetSources();
        Task<IEnumerable<FirmDto>> GetFirms();
        Task<IEnumerable<LocationDto>> GetLocations();
        Task<IEnumerable<PracticeAreaDto>> GetPracticeAreas();
        Task<IEnumerable<PQEDto>> GetPQEs();
    }

    public class AppService : IAppService
    {

        private readonly ISQLRepository _repo;
        private readonly IConnection _conn;

        public AppService(string connection)
        {
            /*CONTAINER???*/
            _conn = new Connection(connection);
            _repo = new SQLRepository(_conn);
        }


    
        #region GETS

        /// <summary>
        /// Gets All Contacts Added by User
        /// </summary>
        /// <returns>Strongly Typed List of Contacts</returns>
        public async Task<PagedResults<ContactsListViewModel>> GetUserContacts(string user, int page = 1, int pageSize = 6)
        {
            var results = new PagedResults<ContactsListViewModel>();
            var query = "GetContacts";
            return await _repo.WithConnection(async c =>
             {
                 // map the result
                 var multi=  await c.QueryMultipleAsync(query, new {@user =user,
                                                                    @Offset = (page - 1) * pageSize,
                                                                    @PageSize = pageSize
                }, commandType: CommandType.StoredProcedure);

                 results.Items = multi.Read<ContactsListViewModel>().ToList();
                 results.TotalCount = multi.ReadFirst<int>();
                 results.PageIndex = page;
                 results.PageSize = pageSize;
                return results;
            });


        }

        /// <summary>
        /// Get Contacts with Search
        /// </summary>
        /// <param name="user"></param>
        /// <param name="param">search param</param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<PagedResults<ContactsListViewModel>> SearchUserContacts(string user, string param, int page = 1, int pageSize = 6)
        {

            var results = new PagedResults<ContactsListViewModel>();
            var query = "SearchContacts";
            return await _repo.WithConnection(async c =>
            {
                // map the result
                var multi = await c.QueryMultipleAsync(query, new
                {
                    @user = user,
                    @param = param,
                    @Offset = (page - 1) * pageSize,
                    @PageSize = pageSize
                }, commandType: CommandType.StoredProcedure);

                results.Items = multi.Read<ContactsListViewModel>().ToList();
                results.TotalCount = multi.ReadFirst<int>();
                results.PageIndex = page;
                results.PageSize = pageSize;
                return results;
            });







        }


        public async Task<ContactDto> GetContact(int id)
        {
            var query = "GetContactById";
            return await _repo.WithConnection(async c =>
            {
                // map the result
                var result = await c.QueryFirstOrDefaultAsync<ContactDto>(query, new { @id = id }, commandType: CommandType.StoredProcedure);
                return result;
            });
        }

        #endregion

        #region POSTS

        /// <summary>
        /// Create New Request
        /// </summary>
        /// <param name="entry">New Entry</param>
        /// <returns>1 if successful</returns>
        public async Task<int> CreateContact(ContactDto entry)
        {
            var query = "CreateContact";
            // execute 
            return await _repo.WithConnection(async c =>
            {
                // map the result from stored procedure to MDT data model
                var result = await c.QueryFirstOrDefaultAsync<int>(query,
                      new
                      {
                          @MatterNo = entry.MatterNo
                                   ,@ContactName = entry.ContactName
                                   ,@PQE = entry.PQE
                                   ,@Firm = entry.Firm
                                   ,@PracticeArea = entry.PracticeArea
                                   ,@Location = entry.Location
                                   ,@Source = entry.Source
                                   ,@SourceOther = entry.SourceOther
                                   ,@CreatedBy = entry.CreatedBy
                      }
                    , commandType: CommandType.StoredProcedure);
                return result;
            });
        }

        /// <summary>
        /// Update Request
        /// </summary>
        /// <param name="entry">Entry to Updated</param>
        /// <returns>1 if successful</returns>
        public async Task<int> UpdateContact(ContactDto entry)
        {

            var query = "UpdateContact";
            // execute 
            return await _repo.WithConnection(async c =>
            {
                // map the result from stored procedure to MDT data model
                var result = await c.QueryFirstOrDefaultAsync<int>(query,
                               new
                               {
                                    @MatterNo = entry.MatterNo
                                   ,@ContactName = entry.ContactName
                                   ,@PQE = entry.PQE
                                   ,@Firm = entry.Firm
                                   ,@PracticeArea = entry.PracticeArea
                                   ,@Location = entry.Location
                                   ,@Source = entry.Source
                                   ,@SourceOther = entry.SourceOther
                                   ,@ModifiedBy = entry.Modifiedby
                                   ,@Id = entry.Id

                               }
                , commandType: CommandType.StoredProcedure);
                return result;
            });
        }

        /// <summary>
        /// Delete Request
        /// </summary>
        /// <param name="Id">Entry Id been Deleted</param>
        /// <returns>1 if successful</returns>
        public async Task<int> DeleteContact(int Id, string user)
        {

            var query = "DeleteContact";
            // execute 
            return await _repo.WithConnection(async c =>
            {
                // map the result from stored procedure to MDT data model
                var result = await c.QueryFirstOrDefaultAsync<int>(query,
                         new
                         {
                              @Id = Id
                             , @ModifiedBy = user
                         }
                  , commandType: CommandType.StoredProcedure);
                return result;
            });
        }

        /// <summary>
        /// Check if person already exists
        /// </summary>
        /// <param name="person">Person  to be Added</param>
        /// <param name="firm">Firm Name</param>
        /// <returns></returns>
        public async Task<int> CheckIfContactExists(string person, int firm)
        {

            var query = "CheckIfContactAlreadyExists";
            // execute 
            return await _repo.WithConnection(async c =>
            {
                // map the result from stored procedure to MDT data model
                var result = await c.QueryFirstOrDefaultAsync<int>(query,
                               new
                               {
                                    @Person = person
                                   ,@Firm =firm

                               }
                , commandType: CommandType.StoredProcedure);
                return result;
            });
        }




        /// <summary>
        /// Sources List
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SourceDto>> GetSources()
        {
            var query = "GetSources";
            return await _repo.WithConnection(async c =>
            {
                // map the result
                var results = await c.QueryAsync<SourceDto>(query, commandType: CommandType.StoredProcedure);
                return results;
            });
        }


        /// <summary>
        /// Firms List
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<FirmDto>> GetFirms()
        {
            var query = "GetFirms";
            return await _repo.WithConnection(async c =>
            {
                // map the result
                var results = await c.QueryAsync<FirmDto>(query, commandType: CommandType.StoredProcedure);
                return results;
            });
        }


        public async Task<IEnumerable<LocationDto>> GetLocations()
        {
            var query = "GetLocations";
            return await _repo.WithConnection(async c =>
            {
                // map the result
                var results = await c.QueryAsync<LocationDto>(query, commandType: CommandType.StoredProcedure);
                return results;
            });
        }


        public async Task<IEnumerable<PracticeAreaDto>> GetPracticeAreas()
        {
            var query = "GetPracticeAreas";
            return await _repo.WithConnection(async c =>
            {
                // map the result
                var results = await c.QueryAsync<PracticeAreaDto>(query, commandType: CommandType.StoredProcedure);
                return results;
            });
        }



        public async Task<IEnumerable<PQEDto>> GetPQEs()
        {
            var query = "GetPQEs";
            return await _repo.WithConnection(async c =>
            {
                // map the result
                var results = await c.QueryAsync<PQEDto>(query, commandType: CommandType.StoredProcedure);
                return results;
            });
        }
        #endregion
    }
}
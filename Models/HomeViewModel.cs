using System.Collections.Generic;
using Infrastructure.Objects.Dtos;

namespace ContactsDirectory.Models
{
    /// <summary>
    /// This will hold all home page view data
    /// </summary>
    public class HomeViewModel
    {
        public IEnumerable<ContactDto> Contacts { get; set; }
        //usually viewmodel will contain many properties
    }


    public class LayoutViewModel
    {
        public string SearchParam { get; set; }
    }
}

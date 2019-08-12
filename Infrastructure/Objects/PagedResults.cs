using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Objects
{
    /***
     * 
     * Creating a Paged Result Set
     * T is the collection we want to return
     * 
     * */
    public class PagedResults<T>
    {
        public IEnumerable<T> Items { get; set; }

        public int TotalCount { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get;  set; }

        public int TotalPages => (int) Math.Ceiling(TotalCount / (double) PageSize);

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Objects.Dtos
{
  public class RequestDto
    {

        public int Id { get; set; }
        public int Type { get; set; }
        public int Category { get; set; }
        public int Priority { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public string OwnedBy { get; set; }
        public string LoggedBy { get; set; }
        public DateTime LoggedAt { get; set; }

    }


    public class RequestStatusesDto
    {
        public int Id { get; set; }
        public string Status { get; set; }
    }


    public class RequestPrioritiesDto
    {
        public int Id { get; set; }
        public string Priority { get; set; }
    }


    public class RequestCategoriesDto
    {
        public int Id { get; set; }
        public string Category { get; set; }
    }


}

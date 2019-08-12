using System;

namespace Infrastructure.Objects.Dtos
{
    /// <summary>
    /// Person Object based on People Finder
    /// </summary>
    public class ContactDto
    {
        public int Id { get; set; }
        public string MatterNo { get; set; }
        public string ContactName { get; set; }
        public int PQE { get; set; }
        public int Firm { get; set; }
        public int PracticeArea { get; set; }
        public int Location { get; set; }
        public int Source { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Modifiedby { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string SourceOther { get; set; }

    }







    //Lookups
    public class SourceDto
    {
        public int Id { get; set; }
        public string Source { get; set; }
    }


    public class FirmDto
    {
        public int Id { get; set; }
        public string Firm { get; set; }
    }


    public class LocationDto
    {
        public int Id { get; set; }
        public string Location { get; set; }
    }

    public class PracticeAreaDto
    {
        public int Id { get; set; }
        public string PracticeArea { get; set; }
    }


    public class PQEDto
    {
        public int Id { get; set; }
        public string PQE { get; set; }
    }
}

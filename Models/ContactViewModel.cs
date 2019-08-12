using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContactsDirectory.Models
{
    public class ContactViewModel
    {
        [Key]
        public int Id { get; set; }

        [StringLength(150)]
        public string MatterNo { get; set; }


        [Required]
        [StringLength(100)]
        public string ContactName { get; set; }


        [Required]
        public int PQE { get; set; }
        [Required]
        public int Firm { get; set; }
        [Required]
        public int PracticeArea { get; set; }
        [Required]
        public int Source { get; set; }
        [Required]
        public int Location { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Modifiedby { get; set; }
        public DateTime ModifiedOn { get; set; }

        [StringLength(150)]
        public string SourceOther { get; set; }
    }

    public class ContactsListViewModel
    {
        public int Id { get; set; }
        public string MatterNo { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactNumber { get; set; }
        public string PQE { get; set; }
        public string Firm { get; set; }
        public string PracticeArea { get; set; }
        public string Location { get; set; }
        public string Source { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Modifiedby { get; set; }
        public DateTime ModifiedOn { get; set; }


    }
}

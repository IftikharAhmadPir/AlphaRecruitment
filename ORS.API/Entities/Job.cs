using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ORS.API.Entities
{
    public partial class Job
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string JobTitle { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }
        public Nullable<DateTime> DatePosted { get; set; }
        public Nullable<System.DateTime> DateOpening { get; set; }
        public Nullable<System.DateTime> DateClosing { get; set; }
        public string ContactInfo { get; set; }
        public Nullable<decimal> Pay { get; set; }
        public string Employer_Id { get; set; }

        [ForeignKey("Employer_Id")]
        public virtual Employer Employers { get; set; }

        public ICollection<Application> Applications { get; set; }

    }
}

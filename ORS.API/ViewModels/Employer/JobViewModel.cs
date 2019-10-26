using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ORS.API.ViewModels
{
    public class JobViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage="Job Title is Required")]
        public string JobTitle { get; set; }
        public string City { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }

        public Nullable<DateTime> DatePosted = DateTime.Now;
        public Nullable<DateTime> DateOpening { get; set; }
        
        [Required(ErrorMessage = "Closing Date Required")]
        public Nullable<DateTime> DateClosing { get; set; }
        public string ContactInfo { get; set; }
        public Nullable<decimal> Pay { get; set; }
        public string Employer_Id { get; set; }

    }
}
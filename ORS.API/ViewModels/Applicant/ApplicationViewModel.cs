using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ORS.API.ViewModels
{
    public class ApplicationViewModel
    {
        public int Id { get; set; }
        public string Organization { get; set; }
        public string JobTitle { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public DateTime DateApplied {get; set;}
        public Nullable<int> ShortListed { get; set; }
        public Nullable<System.DateTime> InterviewDate { get; set; }
        public string link { get; set; }
        public IEnumerable<ApplicantViewModel> Applicants { get; set; }
    }
}
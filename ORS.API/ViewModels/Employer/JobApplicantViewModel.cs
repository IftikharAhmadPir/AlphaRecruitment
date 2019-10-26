using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ORS.API.ViewModels
{
    public class JobApplicantViewModel
    {
        public string Id { get; set; }
        public int AppId { get; set; }
        public string ApplicantName { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<int> shortlisted { get; set; }
        public Nullable<System.DateTime> InterviewDate { get; set; }
        public string link { get; set; }
    }
}
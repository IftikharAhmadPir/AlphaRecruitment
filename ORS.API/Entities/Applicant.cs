using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;

namespace ORS.API.Entities
{
    public partial class Applicant : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mstatus { get; set; }
        public string Gender { get; set; }
        public string Objective { get; set; }
        public byte[] picture { get; set; }
        public bool WaitingForInternship { get; set; }
        public string MailAddress { get; set; }

        //public Nullable<System.DateTime> dob { get; set; }
        //public Nullable<int> nationality { get; set; }
        //public Nullable<int> citizenship { get; set; }
        //public Nullable<int> ctoforigin { get; set; }
        //public string hbox { get; set; }
        //public string htown { get; set; }
        //public string hzip_postal { get; set; }
        //public string hphone { get; set; }
        //public string hmobile { get; set; }
        //public string hemail { get; set; }
        //public string obox { get; set; }
        //public string otown { get; set; }
        //public string ozip_postal { get; set; }
        //public string ophone { get; set; }
        //public string omobile { get; set; }
        //public string oemail { get; set; }
        //public string qualsumm { get; set; }

        public ICollection<Experience> Experiences { get; set;}
        public ICollection<Publication> Publications { get; set; }
        public ICollection<Education> Educations { get; set; }
        public ICollection<Reference> References { get; set; }
        public ICollection<Membership> Memberships { get; set; }
        public ICollection<Training> Trainings { get; set; }
        public ICollection<Applicant_Language> Languages { get; set; }
        //public ICollection<Application> Applications { get; set; }
    }
}

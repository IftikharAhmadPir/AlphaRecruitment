using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ORS.API.Entities
{
    public partial class Countries
    {
        public int Id { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }

        public ICollection<Education> Educations { get; set; }
        public ICollection<Employer> Employers { get; set; }
        public ICollection<Admin> Admins { get; set; }
        public ICollection<Applicant> Applicants { get; set; }
        public ICollection<Job> jobs { get; set; }
        public ICollection<City> Cities { get; set; } 
    }
}

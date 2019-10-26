using System;
using System.Collections.Generic;

namespace ORS.API.Entities
{
    public partial class Emptype
    {
        //public Emptype()
        //{
        //    this.jobs = new List<Job>();
        //}

        public int Id { get; set; }
        public string EmploymentType { get; set; }
        public string Description { get; set; }
        
        //public ICollection<Employer> Employers { get; set; }
    }
}

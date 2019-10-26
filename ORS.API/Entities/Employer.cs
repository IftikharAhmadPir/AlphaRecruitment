using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ORS.API.Entities
{
    public partial class Employer : IdentityUser
    {
        public string Organization { get; set; }
        public string MissionStatement { get; set; }
        public string ContactPerson { get; set; }
        public string PersonEmail { get; set; }
        public string JobTitle { get; set; }
        public string Telephone { get; set; }
        public string Extension { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string PoBox { get; set; }
        public string City { get; set; }
        public string Zip_Postal { get; set; }
        public string Website { get; set; }

        public Nullable<int> Countries_Id { get; set; }

        [ForeignKey("Countries_Id")]
        public virtual Countries Countries { get; set; }
        public ICollection<Job> Jobs { get; set; }
    }
}


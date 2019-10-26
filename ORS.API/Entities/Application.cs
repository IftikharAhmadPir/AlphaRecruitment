using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ORS.API.Entities
{
    public partial class Application
    {
        public int Id { get; set; }
        public System.DateTime DateApplied { get; set; }
        public Nullable<int> ShortListed { get; set; }
        public string Applicant_Id { get; set; }

        [ForeignKey("Applicant_Id")]
        public virtual Applicant Applicants { get; set; }
        public int? Job_Id { get; set; }

        [ForeignKey("Job_Id")]
        public virtual Job Jobs { get; set; }

        public ICollection<Interview> Interviews { get; set; }
    }
}

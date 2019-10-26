using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ORS.API.Entities
{
    public class Interview
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> InterviewDate { get; set; }
        public string link { get; set; }
        public int Application_Id { get; set; }

        [ForeignKey("Application_Id")]
        public virtual Application Applications { get; set; }
        public ICollection<InterviewResult> InterViewResults { get; set; }
    }
}
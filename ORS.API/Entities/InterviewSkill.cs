using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ORS.API.Entities
{
    public class InterviewSkill
    {
        public int Id { get; set; }
        public string Skill { get; set; }
        public ICollection<InterviewResult> InterViewResults { get; set; }
    }
}
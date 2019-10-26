using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ORS.API.Entities
{
    public class InterviewResult
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public string Description { get; set; }
        public int Interview_Id { get; set; }
        [ForeignKey("Interview_Id")]
        public virtual Interview Interviews { get; set; }
        public int InterviewSkill_Id { get; set; }
        [ForeignKey("InterviewSkill_Id")]
        public virtual InterviewSkill InterviewSkill { get; set; }
    }
}
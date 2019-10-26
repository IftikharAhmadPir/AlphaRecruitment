using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORS.API.ViewModels
{
    class InterviewResultViewModel
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public string Description { get; set; }
        public int Interview_Id { get; set; }
        public int InterviewSkill_Id { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace ORS.API.Entities
{
    public partial class Training
    {
        public int Id { get; set; }
        public string TrainingTitle { get; set; }
        public string Provider { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public string Description { get; set; }
    }
}

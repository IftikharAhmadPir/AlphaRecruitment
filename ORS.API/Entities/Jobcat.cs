using System;
using System.Collections.Generic;

namespace ORS.API.Entities
{
    public partial class Jobcat
    {
        public int Id { get; set; }
        public string JobCategory { get; set; }
        public ICollection<Job> jobs { get; set; }
    }
}

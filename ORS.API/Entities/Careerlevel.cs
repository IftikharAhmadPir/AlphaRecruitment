using System;
using System.Collections.Generic;

namespace ORS.API.Entities
{
    public partial class careerlevel
    {

        public int Id { get; set; }
        public string Description { get; set; }

        public ICollection<Job> Jobs { get; set; }

    }
}

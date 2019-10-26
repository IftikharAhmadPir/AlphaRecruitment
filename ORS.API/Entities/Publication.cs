using System;
using System.Collections.Generic;

namespace ORS.API.Entities
{
    public partial class Publication
    {
        public int Id { get; set; }
        public string PTitle { get; set; }
        public string Description { get; set; }
        public System.DateTime Pdate { get; set; }        
    }
}

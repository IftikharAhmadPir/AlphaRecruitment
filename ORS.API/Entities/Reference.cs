using System;
using System.Collections.Generic;

namespace ORS.API.Entities
{
    public partial class Reference
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Organization { get; set; }
        public string RefPosition { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Relation { get; set; }
    }
}

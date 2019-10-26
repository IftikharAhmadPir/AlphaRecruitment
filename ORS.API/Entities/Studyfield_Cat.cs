using System;
using System.Collections.Generic;

namespace ORS.API.Entities
{
    public partial class Studyfield_Cat
    {

        public int Id { get; set; }
        public string FieldCategory { get; set; }
        public ICollection<Education> Educations { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace ORS.API.Entities
{
    public partial class Applicant_Language
    {
        public int Id { get; set; }
        public string Language { get; set; }
        public bool Read { get; set; }
        public bool Write { get; set; }
        public bool Speak { get; set; }
    }
}

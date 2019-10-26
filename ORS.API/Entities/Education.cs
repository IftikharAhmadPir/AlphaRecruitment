using System;
using System.Collections.Generic;

namespace ORS.API.Entities
{
    public partial class Education
    {
        public int Id { get; set; }
        public string Degree { get; set; }
        public string Institution { get; set; }
        public string Board_University { get; set; }
        public Nullable<System.DateTime> YearOfGraduation { get; set; }
        public Nullable<System.DateTime> ExpectedGraduation { get; set; }
        public Nullable<int> HighestLevel { get; set; }

        //public string FieldOfStudy { get; set; }
        //public Nullable<int> fieldofstudycategoryid { get; set; }
        //public string City { get; set; }
        //public Nullable<int> AwardCategory { get; set; }
        //public string SpecialAward { get; set; }


    }
}

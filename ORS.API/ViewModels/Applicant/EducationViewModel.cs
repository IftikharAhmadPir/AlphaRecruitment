using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ORS.API.ViewModels
{
    public class EducationViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Degree is Required")]
        public string Degree { get; set; }

        [Required(ErrorMessage = "Institution is Required")]
        public string Institution { get; set; }

        [Required(ErrorMessage = "Board / University is Required")]
        public string Board_University { get; set; }
        public Nullable<System.DateTime> YearOfGraduation { get; set; }
        public Nullable<System.DateTime> ExpectedGraduation { get; set; }

        //public Nullable<int> HighestLevel { get; set; }
    }
}
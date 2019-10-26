using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ORS.API.ViewModels
{
    public class ExperienceViewModel
    {
        public int Id { get; set; }

        [MinLength(5, ErrorMessage = "Job Title has min length of 5 characters")]
        [MaxLength(30,ErrorMessage="Job Title has max length of 30 characters")]
        [Required(ErrorMessage="Job Title is Required")]
        public string JobTitle { get; set; }
        [Required(ErrorMessage = "Duties / Responsibilities Are Required")]
        public string Duties_Responsibilities { get; set; }
        
        [Required(ErrorMessage = "Organization Name is Required")]
        public string Organization { get; set; }
        [Required(ErrorMessage = "Start Date is Required")]
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
    }
}
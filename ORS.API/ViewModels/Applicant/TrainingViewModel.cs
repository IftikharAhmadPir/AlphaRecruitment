using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ORS.API.ViewModels
{
    public class TrainingViewModel
    {



        public int Id { get; set; }
        [Required(ErrorMessage = "Title is Required")]
        public string TrainingTitle { get; set; }
        [Required(ErrorMessage = "Provider is Required")]
        public string Provider { get; set; }
        [Required(ErrorMessage = "Start Date is Required")]
        public System.DateTime StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string Description { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ORS.API.ViewModels
{
    public class InterviewViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Interview Date is Required")]
        public Nullable<System.DateTime> InterviewDate { get; set; }
        [Required(ErrorMessage="Application Id is Missing")]
        public int Application_Id { get; set; }
    }
}
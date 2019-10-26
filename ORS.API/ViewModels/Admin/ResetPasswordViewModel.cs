using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ORS.API.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string email { get; set; }
    }
}
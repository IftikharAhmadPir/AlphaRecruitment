using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ORS.API.ViewModels
{
    public class ReferenceViewModel
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }
        public string Organization { get; set; }
        public string RefPosition { get; set; }
        [Required(ErrorMessage = "Phone Number is Required")]
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Relation { get; set; }
    }
}
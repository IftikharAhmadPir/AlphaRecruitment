using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ORS.API.ViewModels
{
    public class PublicationViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage="Title is Required")]
        public string PTitle { get; set; }
        [Required(ErrorMessage = "Publication Details are Required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Publication Date is Required")]
        public System.DateTime Pdate { get; set; }
    }
}
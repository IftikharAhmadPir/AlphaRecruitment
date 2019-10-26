using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ORS.API.ViewModels
{
    public class LanguageViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Mention Language")]
        public string Language { get; set; }
        public bool Read { get; set; }
        public bool Write { get; set; }
        public bool Speak { get; set; }
    }
}
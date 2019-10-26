using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ORS.API.ViewModels
{
    public class CountriesViewModel
    {
        public int Id { get; set; }
        public string CountryName { get; set; }

//        public string CountryCode { get; set; }

        public IEnumerable<CityViewModel> Cities { get; set; }
    }
}
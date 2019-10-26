using AutoMapper;
using ORS.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;
using System.Data.Entity;
using System.Web.Http.Description;
using System.Threading.Tasks;
using ORS.API.Entities;

namespace ORS.API.Controllers
{
    public class CataloguesController : ApiController
    {
        AuthContext db = new AuthContext();

        [Route("api/countriescat")]
        public IEnumerable<CountriesViewModel> Get()
        {
            var countries = db.Countries.OrderBy(x => x.CountryName);
            var result = Mapper.Map<IEnumerable<CountriesViewModel>>(countries);
            return result;
        }

        [Route("api/citiesbycountry/{countryid}")]
        public IEnumerable<CountriesViewModel> Get(int countryid)
        {
            var cities = Mapper.Map<IEnumerable<CountriesViewModel>>(db.Countries.Where(t => t.Id == countryid).Include(x => x.Cities));
            return cities;
        }

        [Route("api/getinterviewskills")]
        [ResponseType(typeof(InterviewSkill))]
        public IHttpActionResult GetInterviewSkills()
        {
            var result = db.InterviewSkills.ToList();

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}

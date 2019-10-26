using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ORS.API;
using ORS.API.Entities;
using ORS.API.repository;
using AutoMapper;
using ORS.API.ViewModels;

namespace ORS.API.Controllers
{
    public class CountriesController : ApiController
    {
        private AuthContext db = new AuthContext();
        private CountryRepository1 _repo;

        public CountriesController()
        {
            _repo = new CountryRepository1();
        }
        // GET: api/Countries
        public IEnumerable<CountriesViewModel> Get()
        {
            var countries = db.Countries.OrderBy(x => x.CountryName);
            var result = Mapper.Map<IEnumerable<CountriesViewModel>>(countries);
            return result;

            //            return _repo.GetAllCountries();
            //return db.Countries;
        }

        // GET: api/Countries/5
        [ResponseType(typeof(Countries))]
        public async Task<IHttpActionResult> Get(int id)
        {
            Countries countries = await _repo.GetCountryById(id);

            if (countries == null)
            {
                return NotFound();
            }

            return Ok(countries);
        }
//        [Authorize(Roles ="admin")]
        // PUT: api/Countries/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCountries(int id, Countries countries)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != countries.Id)
            {
                return BadRequest();
            }

            db.Entry(countries).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountriesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Countries
        [ResponseType(typeof(Countries))]
        public async Task<IHttpActionResult> PostCountries(Countries countries)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Countries.Add(countries);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = countries.Id }, countries);
        }

        // DELETE: api/Countries/5
    //    [Authorize(Roles ="admin")]
        [ResponseType(typeof(Countries))]
        public async Task<IHttpActionResult> DeleteCountries(int id)
        {
            Countries countries = await db.Countries.FindAsync(id);
            if (countries == null)
            {
                return NotFound();
            }

            db.Countries.Remove(countries);
            await db.SaveChangesAsync();

            return Ok(countries);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CountriesExists(int id)
        {
            return db.Countries.Count(e => e.Id == id) > 0;
        }
    }
}
using ORS.API.Entities;
using ORS.API.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ORS.API.repository
{

    public class CountryRepository1 : IDisposable
    {
        private AuthContext _ctx;

        public CountryRepository1()
        {
            _ctx = new AuthContext();
        }
        public IEnumerable<Countries> GetAllCountries()
        {
            return _ctx.Countries.OrderBy(x => x.CountryName);
        
        }
        public async Task<Countries> GetCountryById(int id)
        {
            return await _ctx.Countries.FindAsync(id);
        }

        public void Dispose()
        {
            _ctx.Dispose();
        }


    }
}
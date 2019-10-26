using ORS.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ORS.API.repository
{
    # region Country Repository

    public class CountryRepository
    {
        public AuthContext _context;
        public CountryRepository(AuthContext context)
        {
            _context = context;
        }

        public IEnumerable<Countries> GetAllCountries()
        {
            return _context.Countries
                .OrderBy(x => x.CountryName)
                .ToList();
        }
    }
    # endregion
}
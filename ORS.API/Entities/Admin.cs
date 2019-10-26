using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ORS.API.Entities
{
    public class Admin : IdentityUser
    {
        public string AdminName { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ORS.API.ViewModels
{
    public class MembershipViewModel
    {
        public int Id { get; set; }
        public string Association { get; set; }
        public string Title_Role { get; set; }
        public System.DateTime MemberSince { get; set; }
    }
}
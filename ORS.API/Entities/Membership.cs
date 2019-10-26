using System;
using System.Collections.Generic;

namespace ORS.API.Entities
{
    public partial class Membership
    {
        public int Id { get; set; }
        public string Association { get; set; }
        public string Title_Role { get; set; }
        public System.DateTime MemberSince { get; set; }
        }
}

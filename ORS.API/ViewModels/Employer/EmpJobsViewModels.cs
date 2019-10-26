using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ORS.API.ViewModels
{
    public class EmpJobsViewModels
    {
        public int Id { get; set; }
        public string JobTitle { get; set; }
        public string City { get; set; }
        public Nullable<DateTime> DatePosted { get; set; }
        public Nullable<DateTime> DateOpening { get; set; }
        public Nullable<DateTime> DateClosing { get; set; }

        //public string Summary { get; set; }
        //public string Description { get; set; }
        //public string Requirements { get; set; }
        //public Nullable<DateTime> DatePosted = DateTime.Now;
        //public Nullable<DateTime> DateOpening { get; set; }
        //public Nullable<DateTime> DateClosing { get; set; }
        //public string ContactInfo { get; set; }
        //public Nullable<decimal> Pay { get; set; }
        public Nullable<decimal> Count { get; set; }
    }
}
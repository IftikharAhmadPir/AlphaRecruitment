using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ORS.API.ViewModels
{
    public class EmpJobApplicationViewModel
    {
        //for job details with applications count
        public int Id { get; set; }
        public string JobTitle { get; set; }
        public Nullable<System.DateTime> DateClosing { get; set; }
        public int NumberOfApps { get; set; }
        
    }
}
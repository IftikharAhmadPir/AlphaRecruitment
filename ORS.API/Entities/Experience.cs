using System;
using System.Collections.Generic;

namespace ORS.API.Entities
{
    public partial class Experience
    {
        public int Id { get; set; }
        public string JobTitle { get; set; }
        public string Duties_Responsibilities { get; set; }
        public string Organization { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }

        //public Nullable<int> StartMonth { get; set; }
        //public Nullable<int> EndMonth { get; set; }
        //public Nullable<decimal> StartSalaryMonth { get; set; }
        //public Nullable<decimal> CurrentSalaryMonth { get; set; }
        //public Nullable<int> Manager_Supervisor { get; set; }

    }
}

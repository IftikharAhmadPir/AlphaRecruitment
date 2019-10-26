using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ORS.API.ViewModels
{
    public partial class EmployerViewModel
    {
        public string Id { get; set; }
        public string Organization { get; set; }
        public string MissionStatement { get; set; }
        public string ContactPerson { get; set; }
        public string PersonEmail { get; set; }
        public string JobTitle { get; set; }
        public string Telephone { get; set; }
        public string Extension { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string PoBox { get; set; }
        public string City { get; set; }
        public string Zip_Postal { get; set; }
        public string Website { get; set; }
        public int? Countries_Id { get; set; }

        public IEnumerable<JobViewModel> Jobs { get; set; }

    }
}

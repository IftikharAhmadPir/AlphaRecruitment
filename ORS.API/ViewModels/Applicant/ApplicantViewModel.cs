using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ORS.API.ViewModels
{
    public class ApplicantViewModel
    {
        [Required(ErrorMessage="First Name is Required")]
        [MinLength(3, ErrorMessage = "Min Length for First Name is 3 Character")]
        [MaxLength(15, ErrorMessage = "Max Length for First Name is 15")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is Required")]
        [MinLength(3, ErrorMessage = "Min Length for Last Name is 3 Character")]
        [MaxLength(15, ErrorMessage = "Max Length for Last Name is 15")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Objective is Required")]
        [MinLength(3, ErrorMessage = "Min Length for Last Name is 50")]
        [MaxLength(500, ErrorMessage = "Max Length for Last Name is 500")]
        public string Objective { get; set; }

        [Required(ErrorMessage = "Marital Status is Required")]
        public string Mstatus { get; set; }
        [Required(ErrorMessage = "Gender is Required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Mailing Address is Required")]
        public string MailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public byte[] picture { get; set; }
        public bool WaitingForInternship { get; set; }
        public IEnumerable<EducationViewModel> Educations { get; set; }
        public IEnumerable<ExperienceViewModel> Experiences { get; set; }
        public IEnumerable<PublicationViewModel> Publications { get; set; }
        public IEnumerable<TrainingViewModel> Trainings { get; set; }
        public IEnumerable<MembershipViewModel> Memberships { get; set; }
        public IEnumerable<LanguageViewModel> Languages { get; set; }
        public IEnumerable<ReferenceViewModel> References { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ORS.API.Entities;
using ORS.API;
using ORS.API.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using AutoMapper;

namespace ORS.API.Controllers
{
    [Authorize(Roles = "jobseeker")]
    public class ApplicantController : ApiController
    {
        private AuthRepository _repo = null;
        private AuthContext db = new AuthContext();
        private UserManager<Applicant> _userManager;
        public ApplicantController()
        {
            _userManager = new UserManager<Applicant>(new UserStore<Applicant>(db));
            _repo = new AuthRepository();
        }

        #region Personal Information
        [Route("api/personalinfo/{id}")]

        [ResponseType(typeof(ApplicantViewModel))]
        public async Task<IHttpActionResult> GetApplicant(string id)
        {
            try
            {
                Applicant obj = new Applicant();
                Applicant result = await db.Applicants.FindAsync(id);

                if (result == null)
                {
                    return NotFound();
                }
                ApplicantViewModel applicant = new ApplicantViewModel();
                applicant.FirstName = result.FirstName;
                applicant.LastName = result.LastName;
                applicant.Objective = result.Objective;
                applicant.WaitingForInternship = result.WaitingForInternship;
                applicant.picture = result.picture;
                applicant.Gender = result.Gender;
                applicant.Mstatus = result.Mstatus;
                applicant.PhoneNumber = result.PhoneNumber;
                applicant.MailAddress = result.MailAddress;
                

                if (applicant == null)
                {
                    return NotFound();
                }

                return Ok(applicant);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);

            }

        }

        [Route("api/{applicantid}/addpersonalinfo")]
        public async Task<IHttpActionResult> PostPersonalInfo(string applicantid, ApplicantViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Applicant applicant = await db.Applicants.FindAsync(applicantid);
                    applicant.FirstName = vm.FirstName;
                    applicant.LastName = vm.LastName;
                    applicant.Objective = vm.Objective;
                    applicant.WaitingForInternship = vm.WaitingForInternship;
                    applicant.picture = vm.picture;
                    applicant.Gender = vm.Gender;
                    applicant.Mstatus = vm.Mstatus;
                    applicant.PhoneNumber = vm.PhoneNumber;
                    applicant.MailAddress = vm.MailAddress;

                    await _userManager.UpdateAsync(applicant);
                    return StatusCode(HttpStatusCode.OK);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/emailconnfirm/{id}")]
        [ResponseType(typeof(string))]
        public async Task<IHttpActionResult> GetEmailConfirmationToken(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                var provider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("ORS");
                _userManager.UserTokenProvider = new Microsoft.AspNet.Identity.Owin.DataProtectorTokenProvider<Applicant>(provider.Create("EmailConfirmation"));
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user.Id);

                return Ok(token);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        #endregion

        #region Education Information

        [Route("api/{applicantid}/addeducationinfo")]
        public async Task<IHttpActionResult> Post(string applicantid, EducationViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Education data = Mapper.Map<Education>(vm);
                    Applicant applicant = await db.Applicants.Include(t => t.Educations).FirstOrDefaultAsync(t => t.Id == applicantid);

                    if (applicant == null)
                    {
                        return BadRequest();
                    }

                    applicant.Educations.Add(data);
                    await db.SaveChangesAsync();
                    return Ok();
                }
                else
                {

                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/educationinfo/{id}")]
        [ResponseType(typeof(ApplicantViewModel))]
        public IHttpActionResult GetEducation(string id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                else
                {
                    var result = db.Applicants.Where(t => t.Id == id).Include(t => t.Educations);
                    var applicant = Mapper.Map<IEnumerable<ApplicantViewModel>>(result);

                    if (applicant == null)
                    {
                        return NotFound();
                    }

                    return Ok(applicant);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [Route("api/educationbyid/{id}")]
        [ResponseType(typeof(EducationViewModel))]
        public async Task<IHttpActionResult> GetEducationById(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                else
                {
                    Education result = await db.Educations.FindAsync(id);
                    EducationViewModel vm = Mapper.Map<EducationViewModel>(result);

                    if (vm == null)
                    {
                        return NotFound();
                    }

                    return Ok(vm);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/editeducationbyid/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEducationById(int? id, EducationViewModel vm)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                else
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (id != vm.Id)
                    {
                        return BadRequest();
                    }
                    Education result = Mapper.Map<Education>(vm);
                    db.Entry(result).State = EntityState.Modified;

                    try
                    {
                        await db.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EducationExists(id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    return StatusCode(HttpStatusCode.NoContent);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/deleducation/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> DeleteEdu(int? id)
        {
            try
            {
                if (id == null) { return BadRequest(); }
                else
                {
                    Education result = await db.Educations.FindAsync(id);
                    if (result == null)
                    {
                        return NotFound();
                    }
                    db.Educations.Remove(result);
                    await db.SaveChangesAsync();
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #endregion

        #region Experience Information

        [Route("api/{applicantid}/addexperienceinfo")]
        public async Task<IHttpActionResult> Post(string applicantid, ExperienceViewModel vm)
        {
            try
            {
                if (applicantid == null) { return BadRequest(); }
                else
                {
                    if (ModelState.IsValid)
                    {
                        Experience data = Mapper.Map<Experience>(vm);
                        Applicant applicant = await db.Applicants.Include(t => t.Experiences).FirstOrDefaultAsync(t => t.Id == applicantid);

                        if (applicant == null)
                        {
                            return NotFound();
                        }
                        applicant.Experiences.Add(data);
                        var result = await db.SaveChangesAsync();

                        return Ok();
                    }
                    else
                    {

                        return BadRequest(ModelState);
                    }
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/experienceinfo/{id}")]
        [ResponseType(typeof(ApplicantViewModel))]
        public IHttpActionResult GetExperience(string id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                else
                {
                    var result = db.Applicants.Where(t => t.Id == id).Include(t => t.Experiences);
                    if (result == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        var applicant = Mapper.Map<IEnumerable<ApplicantViewModel>>(result);
                        return Ok(applicant);
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [Route("api/experiencebyid/{id}")]
        [ResponseType(typeof(ExperienceViewModel))]
        public async Task<IHttpActionResult> GetExperienceById(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                else
                {
                    Experience result = await db.Experiences.FindAsync(id);
                    if (result == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        ExperienceViewModel vm = Mapper.Map<ExperienceViewModel>(result);
                        return Ok(vm);
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/editexperiencebyid/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutExperienceById(int? id, ExperienceViewModel vm)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                else
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (id != vm.Id)
                    {
                        return BadRequest();
                    }
                    Experience result = Mapper.Map<Experience>(vm);

                    db.Entry(result).State = EntityState.Modified;

                    try
                    {
                        await db.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ExperienceExists(id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return StatusCode(HttpStatusCode.NoContent);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/delexperience/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> DeleteExperience(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                else
                {
                    Experience result = await db.Experiences.FindAsync(id);
                    if (result == null)
                    {
                        return NotFound();
                    }
                    db.Experiences.Remove(result);
                    await db.SaveChangesAsync();
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #endregion

        #region Publication Information

        [Route("api/{applicantid}/addpublicationinfo")]
        public async Task<IHttpActionResult> Post(string applicantid, PublicationViewModel vm)
        {
            try
            {
                if (applicantid == null) { return BadRequest(); }
                else
                {
                    if (ModelState.IsValid)
                    {
                        Publication data = Mapper.Map<Publication>(vm);
                        Applicant applicant = await db.Applicants.Include(t => t.Publications).FirstOrDefaultAsync(t => t.Id == applicantid);

                        if (applicant == null)
                        {
                            return NotFound();
                        }
                        applicant.Publications.Add(data);
                        var result = await db.SaveChangesAsync();

                        return Ok();
                    }
                    else
                    {

                        return BadRequest(ModelState);
                    }
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/publicationinfo/{id}")]
        [ResponseType(typeof(ApplicantViewModel))]
        public IHttpActionResult GetPublication(string id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                else
                {
                    var result = db.Applicants.Where(t => t.Id == id).Include(t => t.Publications);
                    if (result == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        var applicant = Mapper.Map<IEnumerable<ApplicantViewModel>>(result);
                        return Ok(applicant);
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [Route("api/publicationbyid/{id}")]
        [ResponseType(typeof(PublicationViewModel))]
        public async Task<IHttpActionResult> GetPublicationById(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                else
                {
                    Publication result = await db.Publications.FindAsync(id);
                    if (result == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        PublicationViewModel vm = Mapper.Map<PublicationViewModel>(result);
                        return Ok(vm);
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/editpublicationbyid/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPublicationById(int? id, PublicationViewModel vm)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                else
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (id != vm.Id)
                    {
                        return BadRequest();
                    }
                    Publication result = Mapper.Map<Publication>(vm);

                    db.Entry(result).State = EntityState.Modified;

                    try
                    {
                        await db.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!PublicationExists(id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return StatusCode(HttpStatusCode.NoContent);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/delpublication/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> DeletePublication(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                else
                {
                    Publication result = await db.Publications.FindAsync(id);
                    if (result == null)
                    {
                        return NotFound();
                    }
                    db.Publications.Remove(result);
                    await db.SaveChangesAsync();
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #endregion

        #region Reference Information

        [Route("api/{applicantid}/addreferenceinfo")]
        public async Task<IHttpActionResult> Post(string applicantid, ReferenceViewModel vm)
        {
            try
            {
                if (applicantid == null) { return BadRequest(); }
                else
                {
                    if (ModelState.IsValid)
                    {
                        Reference data = Mapper.Map<Reference>(vm);
                        Applicant applicant = await db.Applicants.Include(t => t.References).FirstOrDefaultAsync(t => t.Id == applicantid);

                        if (applicant == null)
                        {
                            return NotFound();
                        }
                        applicant.References.Add(data);
                        var result = await db.SaveChangesAsync();

                        return Ok();
                    }
                    else
                    {

                        return BadRequest(ModelState);
                    }
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/referenceinfo/{id}")]
        [ResponseType(typeof(ApplicantViewModel))]
        public IHttpActionResult GetReference(string id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                else
                {
                    var result = db.Applicants.Where(t => t.Id == id).Include(t => t.References);
                    if (result == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        var applicant = Mapper.Map<IEnumerable<ApplicantViewModel>>(result);
                        return Ok(applicant);
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [Route("api/referencebyid/{id}")]
        [ResponseType(typeof(ReferenceViewModel))]
        public async Task<IHttpActionResult> GetReferenceById(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                else
                {
                    Reference result = await db.References.FindAsync(id);
                    if (result == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        ReferenceViewModel vm = Mapper.Map<ReferenceViewModel>(result);
                        return Ok(vm);
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/editreferencebyid/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutReferenceById(int? id, ReferenceViewModel vm)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                else
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (id != vm.Id)
                    {
                        return BadRequest();
                    }
                    Reference result = Mapper.Map<Reference>(vm);

                    db.Entry(result).State = EntityState.Modified;

                    try
                    {
                        await db.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ReferenceExists(id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return StatusCode(HttpStatusCode.NoContent);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/delreference/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> DeleteReference(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                else
                {
                    Reference result = await db.References.FindAsync(id);
                    if (result == null)
                    {
                        return NotFound();
                    }
                    db.References.Remove(result);
                    await db.SaveChangesAsync();
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #endregion

        #region Training Information

        [Route("api/{applicantid}/addtraininginfo")]
        public async Task<IHttpActionResult> Post(string applicantid, TrainingViewModel vm)
        {
            try
            {
                if (applicantid == null) { return BadRequest(); }
                else
                {
                    if (ModelState.IsValid)
                    {
                        Training data = Mapper.Map<Training>(vm);
                        Applicant applicant = await db.Applicants.Include(t => t.Trainings).FirstOrDefaultAsync(t => t.Id == applicantid);

                        if (applicant == null)
                        {
                            return NotFound();
                        }
                        applicant.Trainings.Add(data);
                        var result = await db.SaveChangesAsync();

                        return Ok();
                    }
                    else
                    {

                        return BadRequest(ModelState);
                    }
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/traininginfo/{id}")]
        [ResponseType(typeof(ApplicantViewModel))]
        public IHttpActionResult GetTraining(string id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                else
                {
                    var result = db.Applicants.Where(t => t.Id == id).Include(t => t.Trainings);
                    if (result == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        var applicant = Mapper.Map<IEnumerable<ApplicantViewModel>>(result);
                        return Ok(applicant);
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [Route("api/trainingbyid/{id}")]
        [ResponseType(typeof(TrainingViewModel))]
        public async Task<IHttpActionResult> GetTrainingById(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                else
                {
                    Training result = await db.Trainings.FindAsync(id);
                    if (result == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        TrainingViewModel vm = Mapper.Map<TrainingViewModel>(result);
                        return Ok(vm);
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/edittrainingbyid/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTrainingById(int? id, TrainingViewModel vm)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                else
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (id != vm.Id)
                    {
                        return BadRequest();
                    }
                    Training result = Mapper.Map<Training>(vm);

                    db.Entry(result).State = EntityState.Modified;

                    try
                    {
                        await db.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TrainingExists(id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return StatusCode(HttpStatusCode.NoContent);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/deltraining/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> DeleteTraining(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                else
                {
                    Training result = await db.Trainings.FindAsync(id);
                    if (result == null)
                    {
                        return NotFound();
                    }
                    db.Trainings.Remove(result);
                    await db.SaveChangesAsync();
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #endregion

        #region Membership Information

        [Route("api/{applicantid}/addmembershipinfo")]
        public async Task<IHttpActionResult> Post(string applicantid, MembershipViewModel vm)
        {
            try
            {
                if (applicantid == null) { return BadRequest(); }
                else
                {
                    if (ModelState.IsValid)
                    {
                        Membership data = Mapper.Map<Membership>(vm);
                        Applicant applicant = await db.Applicants.Include(t => t.Memberships).FirstOrDefaultAsync(t => t.Id == applicantid);

                        if (applicant == null)
                        {
                            return NotFound();
                        }
                        applicant.Memberships.Add(data);
                        var result = await db.SaveChangesAsync();
                        return Ok();
                    }
                    else
                    {

                        return BadRequest(ModelState);
                    }
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/membershipinfo/{id}")]
        [ResponseType(typeof(ApplicantViewModel))]
        public IHttpActionResult GetMembership(string id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                else
                {
                    var result = db.Applicants.Where(t => t.Id == id).Include(t => t.Memberships);
                    if (result == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        var applicant = Mapper.Map<IEnumerable<ApplicantViewModel>>(result);
                        return Ok(applicant);
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [Route("api/membershipbyid/{id}")]
        [ResponseType(typeof(MembershipViewModel))]
        public async Task<IHttpActionResult> GetMembershipById(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                else
                {
                    Membership result = await db.Memberships.FindAsync(id);
                    if (result == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        MembershipViewModel vm = Mapper.Map<MembershipViewModel>(result);
                        return Ok(vm);
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/editmembershipbyid/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMembershipById(int? id, MembershipViewModel vm)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                else
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (id != vm.Id)
                    {
                        return BadRequest();
                    }
                    Membership result = Mapper.Map<Membership>(vm);

                    db.Entry(result).State = EntityState.Modified;

                    try
                    {
                        await db.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!MembershipExists(id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return StatusCode(HttpStatusCode.NoContent);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/delmembership/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> DeleteMembership(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                else
                {
                    Membership result = await db.Memberships.FindAsync(id);
                    if (result == null)
                    {
                        return NotFound();
                    }
                    db.Memberships.Remove(result);
                    await db.SaveChangesAsync();
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #endregion

        #region Language Information

        [Route("api/{applicantid}/addlanguageinfo")]
        public async Task<IHttpActionResult> Post(string applicantid, LanguageViewModel vm)
        {
            try
            {
                if (applicantid == null) { return BadRequest(); }
                else
                {
                    if (ModelState.IsValid)
                    {
                        Applicant_Language data = Mapper.Map<Applicant_Language>(vm);
                        Applicant applicant = await db.Applicants.Include(t => t.Languages).FirstOrDefaultAsync(t => t.Id == applicantid);

                        if (applicant == null)
                        {
                            return NotFound();
                        }
                        applicant.Languages.Add(data);
                        var result = await db.SaveChangesAsync();
                        return Ok();
                    }
                    else
                    {
                        return BadRequest(ModelState);
                    }
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/languageinfo/{id}")]
        [ResponseType(typeof(ApplicantViewModel))]
        public IHttpActionResult GetLanguage(string id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                else
                {
                    var result = db.Applicants.Where(t => t.Id == id).Include(t => t.Languages);
                    if (result == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        var applicant = Mapper.Map<IEnumerable<ApplicantViewModel>>(result);
                        return Ok(applicant);
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [Route("api/languagebyid/{id}")]
        [ResponseType(typeof(LanguageViewModel))]
        public async Task<IHttpActionResult> GetLanguageById(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                else
                {
                    Applicant_Language result = await db.Languages.FindAsync(id);
                    if (result == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        LanguageViewModel vm = Mapper.Map<LanguageViewModel>(result);
                        return Ok(vm);
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/editlanguagebyid/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutLanguageById(int? id, LanguageViewModel vm)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                else
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (id != vm.Id)
                    {
                        return BadRequest();
                    }
                    Applicant_Language result = Mapper.Map<Applicant_Language>(vm);

                    db.Entry(result).State = EntityState.Modified;

                    try
                    {
                        await db.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ALanguageExists(id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return StatusCode(HttpStatusCode.NoContent);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/dellanguage/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> DeleteLanguage(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                else
                {
                    Applicant_Language result = await db.Languages.FindAsync(id);
                    if (result == null)
                    {
                        return NotFound();
                    }
                    db.Languages.Remove(result);
                    await db.SaveChangesAsync();
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #endregion

        #region Jobs Information

        [Route("api/jobinfo/{userid}")]
        [ResponseType(typeof(JobViewModel))]
        public IHttpActionResult GetJobs(string userid)
        {
            try
            {

                var result = db.Jobs.Where(t => t.DateClosing > System.DateTime.Today)
                                .GroupJoin(db.Applications.Where(t => t.Applicant_Id == userid), job => job.Id, app => app.Job_Id, (job, app) => new { job, app })
                                    .GroupJoin(db.Employers, job => job.job.Employer_Id, emp => emp.Id, (job, emp) => new JobSearchViewModel
                                    {
                                        Id = job.job.Id,
                                        JobTitle = job.job.JobTitle,
                                        Description = job.job.Description,
                                        City = job.job.City,
                                        DateOpening = job.job.DateOpening,
                                        DateClosing = job.job.DateClosing,
                                        Organization = emp.FirstOrDefault().Organization,
                                        applied = job.app.FirstOrDefault().Applicant_Id == userid? true : false
                                    });
                //var result = db.Jobs.Where(t => t.DateClosing > System.DateTime.Today);
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    //var jobs = Mapper.Map<IEnumerable<JobViewModel>>(result);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [Route("api/jobdetails/{id}")]
        [ResponseType(typeof(JobViewModel))]
        public async Task<IHttpActionResult> GetJobById(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                else
                {
                    Job result = await db.Jobs.FindAsync(id);
                    if (result == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        JobViewModel vm = Mapper.Map<JobViewModel>(result);
                        return Ok(vm);
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/applyforjob/{applicantid}/{jobid}")]
        public async Task<IHttpActionResult> Post(string applicantid, int? jobid)
        {
            try
            {
                if (applicantid == null || jobid == null) { return BadRequest(); }
                else
                {
                    var application = db.Applications.Where(t => t.Job_Id == jobid && t.Applicant_Id == applicantid);
                    if (application.Count() < 1)
                    {
                        db.Applications.Add(new Application { DateApplied = System.DateTime.Now, Applicant_Id = applicantid, Job_Id = jobid });
                        var result = await db.SaveChangesAsync();
                        return Ok();
                    }
                    else
                    {
                        return Conflict();

                    }
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        #endregion

        #region Applications Information


        [Route("api/userappsinfo/{userid}")]
        [ResponseType(typeof(ApplicationViewModel))]
        public IHttpActionResult GetUserAppsInfo(string userid)
        {
            try
            {
                if (userid == null)
                {
                    return BadRequest();
                }
                else
                {
                    var result = db.Jobs
                        .Join(db.Applications.Where(x => x.Applicant_Id == userid), job => job.Id, app => app.Job_Id, (job, app) => new { job, app })
                        .GroupJoin(db.Employers, job => job.job.Employer_Id, emp => emp.Id, (job, emp) => new ApplicationViewModel
                        {
                            JobTitle = job.job.JobTitle,
                            ShortListed = job.app.ShortListed,
                            Organization = emp.FirstOrDefault().Organization,
                            Description = job.job.Description,
                            Summary = job.job.Summary,
                            Id = job.app.Id,
                            DateApplied = job.app.DateApplied
                        });

                    if (result == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        return Ok(result);
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        #endregion

        #region Interview Information


        [Route("api/userinterviewinfo/{userid}")]
        [ResponseType(typeof(ApplicationViewModel))]
        public IHttpActionResult GetUserInterviewsInfo(string userid)
        {
            try
            {
                if (userid == null)
                {
                    return BadRequest();
                }
                else
                {
                    var result = db.Jobs
                        .Join(db.Applications.Where(x => x.Applicant_Id == userid && x.ShortListed == 2), job => job.Id, app => app.Job_Id, (job, app) => new { job, app })
                        .GroupJoin(db.Employers, job => job.job.Employer_Id, emp => emp.Id, (job, emp) => new { job, emp })
                        .GroupJoin(db.Interviews, job => job.job.app.Id, interview => interview.Application_Id, (job, interview) => new ApplicationViewModel

                        {
                            JobTitle = job.job.job.JobTitle,
                            ShortListed = job.job.app.ShortListed,
                            Organization = job.emp.FirstOrDefault().Organization,
                            Description = job.job.job.Description,
                            Summary = job.job.job.Summary,
                            Id = job.job.app.Id,
                            DateApplied = job.job.app.DateApplied,
                            link = interview.FirstOrDefault().link,
                            InterviewDate = interview.FirstOrDefault().InterviewDate
                        });

                    if (result == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        return Ok(result);
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        #endregion

        #region Profile Picture

#endregion
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private bool ApplicantExists(string id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }
        private bool EducationExists(int? id)
        {
            return db.Educations.Count(e => e.Id == id) > 0;
        }
        private bool ExperienceExists(int? id)
        {
            return db.Experiences.Count(e => e.Id == id) > 0;
        }
        private bool PublicationExists(int? id)
        {
            return db.Publications.Count(e => e.Id == id) > 0;
        }
        private bool ReferenceExists(int? id)
        {
            return db.References.Count(e => e.Id == id) > 0;
        }
        private bool TrainingExists(int? id)
        {
            return db.Trainings.Count(e => e.Id == id) > 0;
        }
        private bool MembershipExists(int? id)
        {
            return db.Memberships.Count(e => e.Id == id) > 0;
        }
        private bool ALanguageExists(int? id)
        {
            return db.Languages.Count(e => e.Id == id) > 0;
        }
    }

}
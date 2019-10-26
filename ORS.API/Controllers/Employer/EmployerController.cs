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
using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Collections;

namespace ORS.API.Controllers
{
    [Authorize(Roles = "company")]
    public class EmployerController : ApiController
    {
        private AuthContext db = new AuthContext();
        private UserManager<Employer> _userManager;

        # region Employer Information
        public EmployerController()
        {
            _userManager = new UserManager<Employer>(new UserStore<Employer>(db));

        }
        // GET api/Employer
        public IEnumerable<EmployerViewModel> GetUsers()
        {
            var result = db.Employers;
            return Mapper.Map<IEnumerable<EmployerViewModel>>(result).ToList();
        }

        // GET api/Employer/5
        [ResponseType(typeof(EmployerViewModel))]
        public async Task<IHttpActionResult> GetEmployer(string id)
        {
            Employer result = await db.Employers.FindAsync(id);

            var employer = Mapper.Map<EmployerViewModel>(result);

            if (employer == null)
            {
                return NotFound();
            }

            return Ok(employer);
        }

        //// PUT api/Employer/5
        //public async Task<IHttpActionResult> PutEmployer(string id, EmployerViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    AuthContext db = new AuthContext();
        //    var user = await _userManager.FindByIdAsync(id);
        //    user.Organization = model.Organization;
        //    user.ContactPerson = model.ContactPerson;
        //    user.JobTitle = model.JobTitle;
        //    user.Mobile = model.Mobile;
        //    user.Telephone = model.Telephone;
        //    user.Extension = model.Extension;
        //    user.Fax = model.Fax;
        //    user.City = model.City;
        //    user.Website = model.Website;
        //    user.PoBox = model.PoBox;
        //    user.Zip_Postal = model.Zip_Postal;
        //    user.PersonEmail = model.PersonEmail;
        //    user.MissionStatement = model.MissionStatement;
        //    user.Countries_Id = model.Countries_Id;

        //    try
        //    {
        //        await _userManager.UpdateAsync(user);
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!EmployerExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.OK);
        //}
        [Route("api/{employerid}/addemployerinfo")]
        public async Task<IHttpActionResult> PostEmployerInfo(string employerid, EmployerViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Employer user = await db.Employers.FindAsync(employerid);
                    user.Organization = model.Organization;
                    user.ContactPerson = model.ContactPerson;
                    user.JobTitle = model.JobTitle;
                    user.Mobile = model.Mobile;
                    user.Telephone = model.Telephone;
                    user.Extension = model.Extension;
                    user.Fax = model.Fax;
                    user.City = model.City;
                    user.Website = model.Website;
                    user.PoBox = model.PoBox;
                    user.Zip_Postal = model.Zip_Postal;
                    user.PersonEmail = model.PersonEmail;
                    user.MissionStatement = model.MissionStatement;
                    user.Countries_Id = model.Countries_Id;

                    try
                    {
                        await _userManager.UpdateAsync(user);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EmployerExists(employerid))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
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
        // POST api/Employer
        [ResponseType(typeof(Employer))]
        public async Task<IHttpActionResult> PostEmployer(Employer employer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Users.Add(employer);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EmployerExists(employer.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = employer.Id }, employer);
        }

        // DELETE api/Employer/5
        [ResponseType(typeof(Employer))]
        public async Task<IHttpActionResult> DeleteEmployer(string id)
        {
            Employer employer = await db.Employers.FindAsync(id);
            if (employer == null)
            {
                return NotFound();
            }

            db.Employers.Remove(employer);
            await db.SaveChangesAsync();

            return Ok(employer);
        }
        private bool EmployerExists(string id)
        {
            return db.Employers.Count(e => e.Id == id) > 0;
        }
        #endregion

        # region Jobs Information

        // GET api/Job
        [Route("api/job/{id}")]
        public IEnumerable<EmployerViewModel> GetJobs(string id)
        {
            var model = Mapper.Map<IEnumerable<EmployerViewModel>>(db.Employers.Where(t => t.Id == id).Include(x => x.Jobs));
            return model;
        }
        [Route("api/jobbyid/{id}")]
        public IEnumerable<JobViewModel> GetJobs(int id)
        {
            var model = Mapper.Map<IEnumerable<JobViewModel>>(db.Jobs.Where(t => t.Id == id));
            return model;
        }

        [ResponseType(typeof(JobViewModel))]

        [Route("api/{employerid}/Job")]
        public async Task<IHttpActionResult> PostJobs(string employerid, JobViewModel job)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {

                    Job newjob = Mapper.Map<Job>(job);
                    Employer employer = await db.Employers.Include(x => x.Jobs).FirstOrDefaultAsync(x => x.Id == employerid);
                    if (employer == null)
                    {
                        return BadRequest();
                    }
                    employer.Jobs.Add(newjob);
                    await db.SaveChangesAsync();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [Route("api/editjobbyid/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutJobById(int? id, JobViewModel vm)
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
                    Job result = Mapper.Map<Job>(vm);
                    db.Entry(result).State = EntityState.Modified;

                    try
                    {
                        await db.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!JobExists(id))
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

        [Route("api/deljob/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> DeleteJob(int? id)
        {
            try
            {
                if (id == null) { return BadRequest(); }
                else
                {
                    Job result = await db.Jobs.FindAsync(id);
                    if (result == null)
                    {
                        return NotFound();
                    }
                    db.Jobs.Remove(result);
                    await db.SaveChangesAsync();
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        private bool JobExists(int? id)
        {
            return db.Jobs.Count(e => e.Id == id) > 0;
        }
        #endregion

        # region Applications Information

        //Get Jobs with application count
        [Route("api/jobwithappscount/{empid}")]
        [ResponseType(typeof(EmpJobApplicationViewModel))]
        public IHttpActionResult GetJobsWithAppsCount(string empid)
        {
            var result = db.Jobs.Where(x => x.Employer_Id == empid)
                        .GroupJoin(db.Applications.Where(x => x.ShortListed == null), job => job.Id, app => app.Job_Id, (job, app) => new EmpJobApplicationViewModel
                        {
                            Id = job.Id,
                            JobTitle = job.JobTitle,
                            DateClosing = job.DateClosing,
                            NumberOfApps = app.Count(x => x.Job_Id == job.Id)
                        });

            return Ok(result);
        }

        //Get Jobs with shortlisted application count
        [Route("api/shortlistedjobwithappscount/{empid}")]
        [ResponseType(typeof(EmpJobApplicationViewModel))]
        public IHttpActionResult GetJobsWithShortlistedAppsCount(string empid)
        {
            var result = db.Jobs.Where(x => x.Employer_Id == empid)
                        .GroupJoin(db.Applications.Where(x => x.ShortListed == 1), job => job.Id, app => app.Job_Id, (job, app) => new EmpJobApplicationViewModel
                        {
                            Id = job.Id,
                            JobTitle = job.JobTitle,
                            DateClosing = job.DateClosing,
                            NumberOfApps = app.Count(x => x.Job_Id == job.Id)
                        });

            return Ok(result);
        }

        //Get Applicants for a specific job
        [Route("api/jobapplicants/{jobid}")]
        [ResponseType(typeof(JobApplicantViewModel))]
        public IHttpActionResult GetJobApplicants(int? jobid)
        {
            try
            {
                if (jobid != null)
                {
                    var result = db.Applicants
                                .Join(db.Applications.Where(x => x.Job_Id == jobid && x.ShortListed == null), applicant => applicant.Id, app => app.Applicant_Id, (applicant, app)
                                => new JobApplicantViewModel
                                {
                                    Id = applicant.Id,
                                    AppId = app.Id,
                                    ApplicantName = applicant.FirstName + " " + applicant.LastName,

                                });

                    return Ok(result);
                }
                else
                { return BadRequest(); }
            }
            catch (Exception ex) { return InternalServerError(ex); }
        }

        //Get Short listed Applicants for a specific job
        [Route("api/jobshortlistedapplicants/{jobid}")]
        [ResponseType(typeof(JobApplicantViewModel))]
        public IHttpActionResult GetJobShortlistedApplicants(int? jobid)
        {
            try
            {
                if (jobid != null)
                {
                    var result = db.Applicants
                                .Join(db.Applications.Where(x => x.Job_Id == jobid && x.ShortListed == 1), applicant => applicant.Id, app => app.Applicant_Id, (applicant, app)
                                => new JobApplicantViewModel
                                {
                                    Id = applicant.Id,
                                    AppId = app.Id,
                                    ApplicantName = applicant.FirstName + " " + applicant.LastName,
                                    shortlisted = app.ShortListed


                                });

                    return Ok(result);
                }
                else
                { return BadRequest(); }
            }
            catch (Exception ex) { return InternalServerError(ex); }
        }

        [Route("api/jobapplicantCv/{applicantid}")]
        [ResponseType(typeof(ApplicantViewModel))]
        public IHttpActionResult GetJobApplicantCv(string applicantid)
        {
            try
            {
                if (applicantid != null)
                {
                    var applicant = db.Applicants
                        .Where(x => x.Id == applicantid)
                        .Include(x => x.Educations)
                        .Include(x => x.Experiences)
                        .Include(x => x.Publications)
                        .Include(x => x.Trainings)
                        .Include(x => x.Memberships)
                        .Include(x => x.Languages)
                        .Include(x => x.References);

                    var result = Mapper.Map<IEnumerable<ApplicantViewModel>>(applicant);
                    return Ok(result);
                }
                else
                { return BadRequest(); }
            }
            catch (Exception ex) { return InternalServerError(ex); }
        }

        [Route("api/shortlistapplicant/{appid}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PostShortListApplicant(int? appid)
        {
            try
            {
                if (appid != null)
                {
                    var application = await db.Applications
                        .Where(x => x.Id == appid).FirstOrDefaultAsync();

                    application.ShortListed = 1;
                    await db.SaveChangesAsync();
                    return Ok(application);
                }
                else
                { return BadRequest(); }
            }
            catch (Exception ex) { return InternalServerError(ex); }
        }

        [Route("api/callforinterview")]
        [ResponseType(typeof(InterviewViewModel))]
        public async Task<IHttpActionResult> PostCallForInterview(InterviewViewModel vm)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest(); }
                else
                {
                    //fir update application status 
                    var application = await db.Applications
                        .Where(x => x.Id == vm.Application_Id).FirstOrDefaultAsync();
                    application.ShortListed = 2;
                    await db.SaveChangesAsync();

                    // than add interview information
                    Interview interview = Mapper.Map<Interview>(vm);
                    interview.link = System.Configuration.ConfigurationManager.AppSettings["ConferenceServer"] + RandomPassword.GenerateLink(50);
                    var result = db.Interviews.Add(interview);
                    await db.SaveChangesAsync();
                    return Ok(application);
                }
            }
            catch (Exception ex) { return InternalServerError(ex); }
        }

        #endregion

        #region Interviews Information
        //Get Jobs with application count with pending interviews
        [Route("api/jobwithappsinterviewscount/{empid}")]
        [ResponseType(typeof(EmpJobApplicationViewModel))]
        public IHttpActionResult GetJobsWithAppsInterviewsCount(string empid)
        {


            var result = db.Jobs.Where(x => x.Employer_Id == empid)
                                .GroupJoin(db.Applications.Where(x => x.ShortListed == 2), job => job.Id, app => app.Job_Id, (job, app) => new EmpJobApplicationViewModel
                        {
                            Id = job.Id,
                            JobTitle = job.JobTitle,
                            DateClosing = job.DateClosing,
                            NumberOfApps = app.Count(x => x.Job_Id == job.Id),
                            
                        });
            return Ok(result);
        }


        //Get Applicants with pending interview for a specific job
        [Route("api/jobapplicantsinterviews/{jobid}")]
        [ResponseType(typeof(JobApplicantViewModel))]
        public IHttpActionResult GetJobApplicantsInterview(int? jobid)
        {
            try
            {
                if (jobid != null)
                {
                    var result = db.Applicants
                                .Join(db.Applications.Where(x => x.Job_Id == jobid && x.ShortListed == 2), applicant => applicant.Id, app => app.Applicant_Id, (applicant, app) => new { applicant, app })
                                .GroupJoin(db.Interviews, application => application.app.Id, interview => interview.Application_Id, (applicant, interview) => new JobApplicantViewModel
                                {
                                    Id = applicant.applicant.Id,
                                    AppId = applicant.app.Id,
                                    ApplicantName = applicant.applicant.FirstName + " " + applicant.applicant.LastName,
                                    PhoneNumber = applicant.applicant.PhoneNumber,
                                    link = interview.FirstOrDefault().link,
                                    InterviewDate = interview.FirstOrDefault().InterviewDate
                                });

                    return Ok(result);
                }
                else
                { return BadRequest(); }
            }
            catch (Exception ex) { return InternalServerError(ex); }
        }

        //[Route("api/{interviewid}/postinterviewresult")]
        //[ResponseType(typeof(ApplicantViewModel))]
        //public IHttpActionResult POSTInterviewResults(string interviewid, InterviewResultViewModel vm)
        //{
        //    try
        //    {
        //        if (applicantid != null)
        //        {
        //            var applicant = db.Applicants
        //                .Where(x => x.Id == applicantid)
        //                .Include(x => x.Educations)
        //                .Include(x => x.Experiences)
        //                .Include(x => x.Publications)
        //                .Include(x => x.Trainings)
        //                .Include(x => x.Memberships)
        //                .Include(x => x.Languages)
        //                .Include(x => x.References);

        //            var result = Mapper.Map<IEnumerable<ApplicantViewModel>>(applicant);
        //            return Ok(result);
        //        }
        //        else
        //        { return BadRequest(); }
        //    }
        //    catch (Exception ex) { return InternalServerError(ex); }
        //}

        #endregion
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
using ORS.API.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ORS.API
{
    public class ORSDBContext : DbContext
    {
        public ORSDBContext()
            : base("ORSDB")
        {

        }
   
    }
    public class AuthContext : IdentityDbContext
    {
        public AuthContext()
            : base("ORSDB")
        {
     
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Language_Cat> Language_Cats { get; set; }
        public DbSet<Emptype> Emptypes { get; set; }
        public DbSet<Jobcat> Jobcats { get; set; }
        public DbSet<Studyfield_Cat> Studyfield_Cats { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Applicant_Language> Languages { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<Reference> References { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<Employer> Employers { get; set; }
        public DbSet<Countries> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Interview> Interviews { get; set; }
        public DbSet<InterviewResult> InterviewResults { get; set; }
        public DbSet<InterviewSkill> InterviewSkills { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUser>().ToTable("ApplicationUsers").Property(p => p.Id).HasColumnName("UserId");
//            modelBuilder.Entity<Admin>().ToTable("AdminUsers").Property(p => p.Id).HasColumnName("AdminId");
            modelBuilder.Entity<Employer>().ToTable("EmployerUsers").Property(p => p.Id).HasColumnName("EmployerId");
            modelBuilder.Entity<Applicant>().ToTable("ApplicantUsers").Property(p => p.Id).HasColumnName("ApplicantId");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
        }

        public System.Data.Entity.DbSet<ORS.API.Entities.Applicant> Applicants { get; set; }



        

    }
     
}
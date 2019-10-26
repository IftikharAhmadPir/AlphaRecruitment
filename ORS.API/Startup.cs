using ORS.API.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using ORS.API.ViewModels;
using ORS.API.Entities;
using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;



[assembly: OwinStartup(typeof(ORS.API.Startup))]

namespace ORS.API
{
    public class Startup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
        public static GoogleOAuth2AuthenticationOptions googleAuthOptions { get; private set; }
        public static FacebookAuthenticationOptions facebookAuthOptions { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            ConfigureOAuth(app);
            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AuthContext, ORS.API.Migrations.Configuration>());

            Mapper.Initialize(configration => {
                configration.CreateMap<Employer, EmployerViewModel>().ReverseMap();
                configration.CreateMap<Job, JobViewModel>().ReverseMap();
                configration.CreateMap<Countries, CountriesViewModel>().ReverseMap();
                configration.CreateMap<City, CityViewModel>().ReverseMap();
                configration.CreateMap<Education, EducationViewModel>().ReverseMap();
                configration.CreateMap<Applicant, ApplicantViewModel>().ReverseMap();
                configration.CreateMap<Experience, ExperienceViewModel>().ReverseMap();
                configration.CreateMap<Publication, PublicationViewModel>().ReverseMap();
                configration.CreateMap<Reference, ReferenceViewModel>().ReverseMap();
                configration.CreateMap<Training, TrainingViewModel>().ReverseMap();
                configration.CreateMap<Membership, MembershipViewModel>().ReverseMap();
                configration.CreateMap<Applicant_Language, LanguageViewModel>().ReverseMap();

                configration.CreateMap<Job, ApplicationViewModel>().ReverseMap();
                configration.CreateMap<Application, ApplicationViewModel>().ReverseMap();
                configration.CreateMap<JobViewModel, ApplicationViewModel>().ReverseMap();
                
                configration.CreateMap<Interview, InterviewViewModel>().ReverseMap();
                configration.CreateMap<InterviewResult, InterviewResultViewModel>().ReverseMap();
                configration.CreateMap<InterviewSkill, InterviewSkillViewModel>().ReverseMap();
            });
        }
        

        public void ConfigureOAuth(IAppBuilder app)
        {
            //use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions() {
            
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = new AuthorizationServerProvider(),
                RefreshTokenProvider = new RefreshTokenProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);

            //Configure Google External Login
            googleAuthOptions = new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "xxxxxx",
                ClientSecret = "xxxxxx",
                Provider = new GoogleAuthProvider()
            };
            app.UseGoogleAuthentication(googleAuthOptions);

            //Configure Facebook External Login
            facebookAuthOptions = new FacebookAuthenticationOptions()
            {
                AppId = "xxxxxx",
                AppSecret = "xxxxxx",
                Provider = new FacebookAuthProvider()
            };
            app.UseFacebookAuthentication(facebookAuthOptions);

        }

        
    }

}
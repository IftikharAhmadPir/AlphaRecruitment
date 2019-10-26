namespace ORS.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationUsers",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        AdminName = c.String(),
                        Discriminator = c.String(maxLength: 128),
                        Countries_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Countries", t => t.Countries_Id1)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Countries_Id1);
            
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Educations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Degree = c.String(),
                        Institution = c.String(),
                        Board_University = c.String(),
                        YearOfGraduation = c.DateTime(),
                        ExpectedGraduation = c.DateTime(),
                        HighestLevel = c.Int(),
                        Applicant_Id = c.String(maxLength: 128),
                        Countries_Id = c.Int(),
                        Studyfield_Cat_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicantUsers", t => t.Applicant_Id)
                .ForeignKey("dbo.Countries", t => t.Countries_Id)
                .ForeignKey("dbo.Studyfield_Cat", t => t.Studyfield_Cat_Id)
                .Index(t => t.Applicant_Id)
                .Index(t => t.Countries_Id)
                .Index(t => t.Studyfield_Cat_Id);
            
            CreateTable(
                "dbo.Experiences",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JobTitle = c.String(),
                        Duties_Responsibilities = c.String(),
                        Organization = c.String(),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        Applicant_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicantUsers", t => t.Applicant_Id)
                .Index(t => t.Applicant_Id);
            
            CreateTable(
                "dbo.Applicant_Language",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Language = c.String(),
                        Read = c.Boolean(nullable: false),
                        Write = c.Boolean(nullable: false),
                        Speak = c.Boolean(nullable: false),
                        Applicant_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicantUsers", t => t.Applicant_Id)
                .Index(t => t.Applicant_Id);
            
            CreateTable(
                "dbo.UserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.ApplicationUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Memberships",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Association = c.String(),
                        Title_Role = c.String(),
                        MemberSince = c.DateTime(nullable: false),
                        Applicant_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicantUsers", t => t.Applicant_Id)
                .Index(t => t.Applicant_Id);
            
            CreateTable(
                "dbo.Publications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PTitle = c.String(),
                        Description = c.String(),
                        Pdate = c.DateTime(nullable: false),
                        Applicant_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicantUsers", t => t.Applicant_Id)
                .Index(t => t.Applicant_Id);
            
            CreateTable(
                "dbo.References",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Organization = c.String(),
                        RefPosition = c.String(),
                        Telephone = c.String(),
                        Email = c.String(),
                        Relation = c.String(),
                        Applicant_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicantUsers", t => t.Applicant_Id)
                .Index(t => t.Applicant_Id);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.ApplicationUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Trainings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TrainingTitle = c.String(),
                        Provider = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Description = c.String(),
                        Applicant_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicantUsers", t => t.Applicant_Id)
                .Index(t => t.Applicant_Id);
            
            CreateTable(
                "dbo.Applications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateApplied = c.DateTime(nullable: false),
                        ShortListed = c.Int(),
                        Applicant_Id = c.String(maxLength: 128),
                        Job_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicantUsers", t => t.Applicant_Id)
                .ForeignKey("dbo.Jobs", t => t.Job_Id)
                .Index(t => t.Applicant_Id)
                .Index(t => t.Job_Id);
            
            CreateTable(
                "dbo.Interviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InterviewDate = c.DateTime(),
                        link = c.String(),
                        Application_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Applications", t => t.Application_Id, cascadeDelete: true)
                .Index(t => t.Application_Id);
            
            CreateTable(
                "dbo.InterviewResults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Interview_Id = c.Int(nullable: false),
                        InterviewSkill_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Interviews", t => t.Interview_Id, cascadeDelete: true)
                .ForeignKey("dbo.InterviewSkills", t => t.InterviewSkill_Id, cascadeDelete: true)
                .Index(t => t.Interview_Id)
                .Index(t => t.InterviewSkill_Id);
            
            CreateTable(
                "dbo.InterviewSkills",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Skill = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        City = c.String(),
                        JobTitle = c.String(),
                        Summary = c.String(),
                        Description = c.String(),
                        Requirements = c.String(),
                        DatePosted = c.DateTime(),
                        DateOpening = c.DateTime(),
                        DateClosing = c.DateTime(),
                        ContactInfo = c.String(),
                        Pay = c.Decimal(precision: 18, scale: 2),
                        Employer_Id = c.String(maxLength: 128),
                        Countries_Id = c.Int(),
                        Jobcat_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.Countries_Id)
                .ForeignKey("dbo.EmployerUsers", t => t.Employer_Id)
                .ForeignKey("dbo.Jobcats", t => t.Jobcat_Id)
                .Index(t => t.Employer_Id)
                .Index(t => t.Countries_Id)
                .Index(t => t.Jobcat_Id);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CountryName = c.String(),
                        CountryCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CityName = c.String(),
                        Countries_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.Countries_Id)
                .Index(t => t.Countries_Id);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Secret = c.String(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        ApplicationType = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        RefreshTokenLifeTime = c.Int(nullable: false),
                        AllowedOrigin = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Emptypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmploymentType = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Jobcats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JobCategory = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Language_Cat",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Language = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RefreshTokens",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Subject = c.String(nullable: false, maxLength: 50),
                        ClientId = c.String(nullable: false, maxLength: 50),
                        IssuedUtc = c.DateTime(nullable: false),
                        ExpiresUtc = c.DateTime(nullable: false),
                        ProtectedTicket = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.Studyfield_Cat",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FieldCategory = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplicantUsers",
                c => new
                    {
                        ApplicantId = c.String(nullable: false, maxLength: 128),
                        Countries_Id = c.Int(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Mstatus = c.String(),
                        Gender = c.String(),
                        Objective = c.String(),
                        picture = c.Binary(),
                        WaitingForInternship = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ApplicantId)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicantId)
                .ForeignKey("dbo.Countries", t => t.Countries_Id)
                .Index(t => t.ApplicantId)
                .Index(t => t.Countries_Id);
            
            CreateTable(
                "dbo.EmployerUsers",
                c => new
                    {
                        EmployerId = c.String(nullable: false, maxLength: 128),
                        Organization = c.String(),
                        MissionStatement = c.String(),
                        ContactPerson = c.String(),
                        PersonEmail = c.String(),
                        JobTitle = c.String(),
                        Telephone = c.String(),
                        Extension = c.String(),
                        Mobile = c.String(),
                        Fax = c.String(),
                        PoBox = c.String(),
                        City = c.String(),
                        Zip_Postal = c.String(),
                        Website = c.String(),
                        Countries_Id = c.Int(),
                    })
                .PrimaryKey(t => t.EmployerId)
                .ForeignKey("dbo.ApplicationUsers", t => t.EmployerId)
                .ForeignKey("dbo.Countries", t => t.Countries_Id)
                .Index(t => t.EmployerId)
                .Index(t => t.Countries_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmployerUsers", "Countries_Id", "dbo.Countries");
            DropForeignKey("dbo.EmployerUsers", "EmployerId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicantUsers", "Countries_Id", "dbo.Countries");
            DropForeignKey("dbo.ApplicantUsers", "ApplicantId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.UserLogins", "UserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.UserClaims", "UserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Educations", "Studyfield_Cat_Id", "dbo.Studyfield_Cat");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Jobs", "Jobcat_Id", "dbo.Jobcats");
            DropForeignKey("dbo.Jobs", "Employer_Id", "dbo.EmployerUsers");
            DropForeignKey("dbo.Jobs", "Countries_Id", "dbo.Countries");
            DropForeignKey("dbo.Educations", "Countries_Id", "dbo.Countries");
            DropForeignKey("dbo.Cities", "Countries_Id", "dbo.Countries");
            DropForeignKey("dbo.ApplicationUsers", "Countries_Id1", "dbo.Countries");
            DropForeignKey("dbo.Applications", "Job_Id", "dbo.Jobs");
            DropForeignKey("dbo.InterviewResults", "InterviewSkill_Id", "dbo.InterviewSkills");
            DropForeignKey("dbo.InterviewResults", "Interview_Id", "dbo.Interviews");
            DropForeignKey("dbo.Interviews", "Application_Id", "dbo.Applications");
            DropForeignKey("dbo.Applications", "Applicant_Id", "dbo.ApplicantUsers");
            DropForeignKey("dbo.Trainings", "Applicant_Id", "dbo.ApplicantUsers");
            DropForeignKey("dbo.References", "Applicant_Id", "dbo.ApplicantUsers");
            DropForeignKey("dbo.Publications", "Applicant_Id", "dbo.ApplicantUsers");
            DropForeignKey("dbo.Memberships", "Applicant_Id", "dbo.ApplicantUsers");
            DropForeignKey("dbo.Applicant_Language", "Applicant_Id", "dbo.ApplicantUsers");
            DropForeignKey("dbo.Experiences", "Applicant_Id", "dbo.ApplicantUsers");
            DropForeignKey("dbo.Educations", "Applicant_Id", "dbo.ApplicantUsers");
            DropIndex("dbo.EmployerUsers", new[] { "Countries_Id" });
            DropIndex("dbo.EmployerUsers", new[] { "EmployerId" });
            DropIndex("dbo.ApplicantUsers", new[] { "Countries_Id" });
            DropIndex("dbo.ApplicantUsers", new[] { "ApplicantId" });
            DropIndex("dbo.Roles", "RoleNameIndex");
            DropIndex("dbo.Cities", new[] { "Countries_Id" });
            DropIndex("dbo.Jobs", new[] { "Jobcat_Id" });
            DropIndex("dbo.Jobs", new[] { "Countries_Id" });
            DropIndex("dbo.Jobs", new[] { "Employer_Id" });
            DropIndex("dbo.InterviewResults", new[] { "InterviewSkill_Id" });
            DropIndex("dbo.InterviewResults", new[] { "Interview_Id" });
            DropIndex("dbo.Interviews", new[] { "Application_Id" });
            DropIndex("dbo.Applications", new[] { "Job_Id" });
            DropIndex("dbo.Applications", new[] { "Applicant_Id" });
            DropIndex("dbo.Trainings", new[] { "Applicant_Id" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.References", new[] { "Applicant_Id" });
            DropIndex("dbo.Publications", new[] { "Applicant_Id" });
            DropIndex("dbo.Memberships", new[] { "Applicant_Id" });
            DropIndex("dbo.UserLogins", new[] { "UserId" });
            DropIndex("dbo.Applicant_Language", new[] { "Applicant_Id" });
            DropIndex("dbo.Experiences", new[] { "Applicant_Id" });
            DropIndex("dbo.Educations", new[] { "Studyfield_Cat_Id" });
            DropIndex("dbo.Educations", new[] { "Countries_Id" });
            DropIndex("dbo.Educations", new[] { "Applicant_Id" });
            DropIndex("dbo.UserClaims", new[] { "UserId" });
            DropIndex("dbo.ApplicationUsers", new[] { "Countries_Id1" });
            DropIndex("dbo.ApplicationUsers", "UserNameIndex");
            DropTable("dbo.EmployerUsers");
            DropTable("dbo.ApplicantUsers");
            DropTable("dbo.Studyfield_Cat");
            DropTable("dbo.Roles");
            DropTable("dbo.RefreshTokens");
            DropTable("dbo.Language_Cat");
            DropTable("dbo.Jobcats");
            DropTable("dbo.Emptypes");
            DropTable("dbo.Clients");
            DropTable("dbo.Cities");
            DropTable("dbo.Countries");
            DropTable("dbo.Jobs");
            DropTable("dbo.InterviewSkills");
            DropTable("dbo.InterviewResults");
            DropTable("dbo.Interviews");
            DropTable("dbo.Applications");
            DropTable("dbo.Trainings");
            DropTable("dbo.UserRoles");
            DropTable("dbo.References");
            DropTable("dbo.Publications");
            DropTable("dbo.Memberships");
            DropTable("dbo.UserLogins");
            DropTable("dbo.Applicant_Language");
            DropTable("dbo.Experiences");
            DropTable("dbo.Educations");
            DropTable("dbo.UserClaims");
            DropTable("dbo.ApplicationUsers");
        }
    }
}

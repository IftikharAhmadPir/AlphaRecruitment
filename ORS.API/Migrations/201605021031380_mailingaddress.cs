namespace ORS.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mailingaddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicantUsers", "MailAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicantUsers", "MailAddress");
        }
    }
}

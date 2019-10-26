namespace ORS.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class second : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InterviewResults", "Ccore", c => c.Int(nullable: false));
            AddColumn("dbo.InterviewResults", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.InterviewResults", "Description");
            DropColumn("dbo.InterviewResults", "Ccore");
        }
    }
}

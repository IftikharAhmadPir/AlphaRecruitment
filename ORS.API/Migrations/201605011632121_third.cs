namespace ORS.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class third : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InterviewResults", "Score", c => c.Int(nullable: false));
            DropColumn("dbo.InterviewResults", "Ccore");
        }
        
        public override void Down()
        {
            AddColumn("dbo.InterviewResults", "Ccore", c => c.Int(nullable: false));
            DropColumn("dbo.InterviewResults", "Score");
        }
    }
}

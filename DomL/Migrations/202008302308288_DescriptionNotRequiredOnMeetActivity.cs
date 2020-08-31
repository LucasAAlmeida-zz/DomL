namespace DomL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DescriptionNotRequiredOnMeetActivity : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MeetActivity", "Description", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MeetActivity", "Description", c => c.String(nullable: false));
        }
    }
}

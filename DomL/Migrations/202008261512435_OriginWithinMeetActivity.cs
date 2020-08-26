namespace DomL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OriginWithinMeetActivity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MeetActivity", "Origin", c => c.String(nullable: false));
            DropColumn("dbo.Person", "Origin");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Person", "Origin", c => c.String());
            DropColumn("dbo.MeetActivity", "Origin");
        }
    }
}

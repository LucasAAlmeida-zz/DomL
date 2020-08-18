namespace DomL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameTables : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Activities", newName: "Activity");
            RenameTable(name: "dbo.AutoActivities", newName: "AutoActivity");
            RenameTable(name: "dbo.BookActivities", newName: "BookActivity");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.BookActivity", newName: "BookActivities");
            RenameTable(name: "dbo.AutoActivity", newName: "AutoActivities");
            RenameTable(name: "dbo.Activity", newName: "Activities");
        }
    }
}

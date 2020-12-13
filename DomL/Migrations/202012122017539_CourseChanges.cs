namespace DomL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CourseChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Course", "Title", c => c.String());
            AddColumn("dbo.Course", "Professor", c => c.String());
            AddColumn("dbo.Course", "Area", c => c.String());
            AddColumn("dbo.Course", "Degree", c => c.String());
            AddColumn("dbo.Course", "Number", c => c.String());
            DropColumn("dbo.Course", "Name");
            DropColumn("dbo.Course", "Teacher");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Course", "Teacher", c => c.String());
            AddColumn("dbo.Course", "Name", c => c.String());
            DropColumn("dbo.Course", "Number");
            DropColumn("dbo.Course", "Degree");
            DropColumn("dbo.Course", "Area");
            DropColumn("dbo.Course", "Professor");
            DropColumn("dbo.Course", "Title");
        }
    }
}

namespace DomL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Book", "Person", c => c.String());
            AddColumn("dbo.Book", "Company", c => c.String());
            AddColumn("dbo.Comic", "Person", c => c.String());
            AddColumn("dbo.Comic", "Company", c => c.String());
            AddColumn("dbo.Course", "Type", c => c.String());
            AddColumn("dbo.Course", "Series", c => c.String());
            AddColumn("dbo.Course", "Person", c => c.String());
            AddColumn("dbo.Course", "Company", c => c.String());
            AddColumn("dbo.Game", "Type", c => c.String());
            DropColumn("dbo.Book", "Author");
            DropColumn("dbo.Book", "Publisher");
            DropColumn("dbo.Comic", "Author");
            DropColumn("dbo.Comic", "Publisher");
            DropColumn("dbo.Course", "Professor");
            DropColumn("dbo.Course", "Area");
            DropColumn("dbo.Course", "Degree");
            DropColumn("dbo.Course", "School");
            DropColumn("dbo.Game", "Platform");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Game", "Platform", c => c.String());
            AddColumn("dbo.Course", "School", c => c.String());
            AddColumn("dbo.Course", "Degree", c => c.String());
            AddColumn("dbo.Course", "Area", c => c.String());
            AddColumn("dbo.Course", "Professor", c => c.String());
            AddColumn("dbo.Comic", "Publisher", c => c.String());
            AddColumn("dbo.Comic", "Author", c => c.String());
            AddColumn("dbo.Book", "Publisher", c => c.String());
            AddColumn("dbo.Book", "Author", c => c.String());
            DropColumn("dbo.Game", "Type");
            DropColumn("dbo.Course", "Company");
            DropColumn("dbo.Course", "Person");
            DropColumn("dbo.Course", "Series");
            DropColumn("dbo.Course", "Type");
            DropColumn("dbo.Comic", "Company");
            DropColumn("dbo.Comic", "Person");
            DropColumn("dbo.Book", "Company");
            DropColumn("dbo.Book", "Person");
        }
    }
}

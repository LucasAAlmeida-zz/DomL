namespace DomL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MeetTables : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE ActivityCategory SET Name='MEET' WHERE Name='PERSON'");

            CreateTable(
                "dbo.MeetActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        PersonId = c.Int(nullable: false),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .ForeignKey("dbo.Person", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.PersonId);
            
            AddColumn("dbo.Person", "Origin", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MeetActivity", "PersonId", "dbo.Person");
            DropForeignKey("dbo.MeetActivity", "Id", "dbo.Activity");
            DropIndex("dbo.MeetActivity", new[] { "PersonId" });
            DropIndex("dbo.MeetActivity", new[] { "Id" });
            DropColumn("dbo.Person", "Origin");
            DropTable("dbo.MeetActivity");

            Sql("UPDATE ActivityCategory SET Name='PERSON' WHERE Name='MEET'");
        }
    }
}

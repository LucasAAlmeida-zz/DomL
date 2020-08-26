namespace DomL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PlayActivity",
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PlayActivity", "PersonId", "dbo.Person");
            DropForeignKey("dbo.PlayActivity", "Id", "dbo.Activity");
            DropIndex("dbo.PlayActivity", new[] { "PersonId" });
            DropIndex("dbo.PlayActivity", new[] { "Id" });
            DropTable("dbo.PlayActivity");
        }
    }
}

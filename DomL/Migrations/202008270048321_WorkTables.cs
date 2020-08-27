namespace DomL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WorkTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorkActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        WorkId = c.Int(nullable: false),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .ForeignKey("dbo.Company", t => t.WorkId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.WorkId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkActivity", "WorkId", "dbo.Company");
            DropForeignKey("dbo.WorkActivity", "Id", "dbo.Activity");
            DropIndex("dbo.WorkActivity", new[] { "WorkId" });
            DropIndex("dbo.WorkActivity", new[] { "Id" });
            DropTable("dbo.WorkActivity");
        }
    }
}

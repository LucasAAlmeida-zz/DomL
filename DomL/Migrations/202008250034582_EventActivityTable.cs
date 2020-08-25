namespace DomL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventActivityTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Description = c.String(nullable: false),
                        IsImportant = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventActivity", "Id", "dbo.Activity");
            DropIndex("dbo.EventActivity", new[] { "Id" });
            DropTable("dbo.EventActivity");
        }
    }
}

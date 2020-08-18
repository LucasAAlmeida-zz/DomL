namespace DomL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AutoTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Auto",
                c => new
                    {
                        AutoId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.AutoId);
            
            CreateTable(
                "dbo.AutoActivities",
                c => new
                    {
                        ActivityId = c.Int(nullable: false),
                        AutoId = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ActivityId)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Auto", t => t.AutoId, cascadeDelete: true)
                .Index(t => t.ActivityId)
                .Index(t => t.AutoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AutoActivities", "AutoId", "dbo.Auto");
            DropForeignKey("dbo.AutoActivities", "ActivityId", "dbo.Activities");
            DropIndex("dbo.AutoActivities", new[] { "AutoId" });
            DropIndex("dbo.AutoActivities", new[] { "ActivityId" });
            DropTable("dbo.AutoActivities");
            DropTable("dbo.Auto");
        }
    }
}

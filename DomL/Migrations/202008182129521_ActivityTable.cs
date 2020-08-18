namespace DomL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActivityTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        ActivityId = c.Int(nullable: false, identity: true),
                        Category = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        DayOrder = c.Int(nullable: false),
                        Classification = c.Int(nullable: false),
                        ActivityBlockId = c.Int(),
                    })
                .PrimaryKey(t => t.ActivityId)
                .ForeignKey("dbo.ActivityBlock", t => t.ActivityBlockId)
                .Index(t => t.ActivityBlockId);
            
            CreateTable(
                "dbo.ActivityBlock",
                c => new
                    {
                        ActivityBlockId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Month = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ActivityBlockId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Activities", "ActivityBlockId", "dbo.ActivityBlock");
            DropIndex("dbo.Activities", new[] { "ActivityBlockId" });
            DropTable("dbo.ActivityBlock");
            DropTable("dbo.Activities");
        }
    }
}

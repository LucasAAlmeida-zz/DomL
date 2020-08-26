namespace DomL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TravelTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TravelActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        TransportId = c.Int(nullable: false),
                        OriginId = c.Int(),
                        DestinationId = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .ForeignKey("dbo.Location", t => t.DestinationId, cascadeDelete: true)
                .ForeignKey("dbo.Location", t => t.OriginId)
                .ForeignKey("dbo.Transport", t => t.TransportId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.TransportId)
                .Index(t => t.OriginId)
                .Index(t => t.DestinationId);
            
            CreateTable(
                "dbo.Location",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Transport",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TravelActivity", "TransportId", "dbo.Transport");
            DropForeignKey("dbo.TravelActivity", "OriginId", "dbo.Location");
            DropForeignKey("dbo.TravelActivity", "DestinationId", "dbo.Location");
            DropForeignKey("dbo.TravelActivity", "Id", "dbo.Activity");
            DropIndex("dbo.TravelActivity", new[] { "DestinationId" });
            DropIndex("dbo.TravelActivity", new[] { "OriginId" });
            DropIndex("dbo.TravelActivity", new[] { "TransportId" });
            DropIndex("dbo.TravelActivity", new[] { "Id" });
            DropTable("dbo.Transport");
            DropTable("dbo.Location");
            DropTable("dbo.TravelActivity");
        }
    }
}

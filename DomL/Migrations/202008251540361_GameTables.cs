namespace DomL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GameTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Franchise",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CreatorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Person", t => t.CreatorId)
                .Index(t => t.CreatorId);
            
            CreateTable(
                "dbo.GameActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        GameId = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .ForeignKey("dbo.Game", t => t.GameId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.GameId);
            
            CreateTable(
                "dbo.Game",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        PlatformId = c.Int(nullable: false),
                        SeriesId = c.Int(),
                        NumberInSeries = c.String(),
                        DirectorId = c.Int(),
                        PublisherId = c.Int(),
                        Score = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Person", t => t.DirectorId)
                .ForeignKey("dbo.MediaType", t => t.PlatformId, cascadeDelete: true)
                .ForeignKey("dbo.Company", t => t.PublisherId)
                .ForeignKey("dbo.Series", t => t.SeriesId)
                .Index(t => t.PlatformId)
                .Index(t => t.SeriesId)
                .Index(t => t.DirectorId)
                .Index(t => t.PublisherId);
            
            CreateTable(
                "dbo.Company",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Series", "FranchiseId", c => c.Int());
            CreateIndex("dbo.Series", "FranchiseId");
            AddForeignKey("dbo.Series", "FranchiseId", "dbo.Franchise", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GameActivity", "GameId", "dbo.Game");
            DropForeignKey("dbo.Game", "SeriesId", "dbo.Series");
            DropForeignKey("dbo.Game", "PublisherId", "dbo.Company");
            DropForeignKey("dbo.Game", "PlatformId", "dbo.MediaType");
            DropForeignKey("dbo.Game", "DirectorId", "dbo.Person");
            DropForeignKey("dbo.GameActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.Series", "FranchiseId", "dbo.Franchise");
            DropForeignKey("dbo.Franchise", "CreatorId", "dbo.Person");
            DropIndex("dbo.Game", new[] { "PublisherId" });
            DropIndex("dbo.Game", new[] { "DirectorId" });
            DropIndex("dbo.Game", new[] { "SeriesId" });
            DropIndex("dbo.Game", new[] { "PlatformId" });
            DropIndex("dbo.GameActivity", new[] { "GameId" });
            DropIndex("dbo.GameActivity", new[] { "Id" });
            DropIndex("dbo.Franchise", new[] { "CreatorId" });
            DropIndex("dbo.Series", new[] { "FranchiseId" });
            DropColumn("dbo.Series", "FranchiseId");
            DropTable("dbo.Company");
            DropTable("dbo.Game");
            DropTable("dbo.GameActivity");
            DropTable("dbo.Franchise");
        }
    }
}

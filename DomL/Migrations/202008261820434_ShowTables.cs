namespace DomL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShowTables : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE ActivityCategory SET Name='SHOW' WHERE Name='SERIES'");

            CreateTable(
                "dbo.ShowActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ShowSeasonId = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .ForeignKey("dbo.ShowSeason", t => t.ShowSeasonId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.ShowSeasonId);
            
            CreateTable(
                "dbo.ShowSeason",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SeriesId = c.Int(nullable: false),
                        Season = c.String(),
                        DirectorId = c.Int(),
                        TypeId = c.Int(),
                        Score = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Person", t => t.DirectorId)
                .ForeignKey("dbo.Series", t => t.SeriesId, cascadeDelete: true)
                .ForeignKey("dbo.MediaType", t => t.TypeId)
                .Index(t => t.SeriesId)
                .Index(t => t.DirectorId)
                .Index(t => t.TypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShowActivity", "ShowSeasonId", "dbo.ShowSeason");
            DropForeignKey("dbo.ShowSeason", "TypeId", "dbo.MediaType");
            DropForeignKey("dbo.ShowSeason", "SeriesId", "dbo.Series");
            DropForeignKey("dbo.ShowSeason", "DirectorId", "dbo.Person");
            DropForeignKey("dbo.ShowActivity", "Id", "dbo.Activity");
            DropIndex("dbo.ShowSeason", new[] { "TypeId" });
            DropIndex("dbo.ShowSeason", new[] { "DirectorId" });
            DropIndex("dbo.ShowSeason", new[] { "SeriesId" });
            DropIndex("dbo.ShowActivity", new[] { "ShowSeasonId" });
            DropIndex("dbo.ShowActivity", new[] { "Id" });
            DropTable("dbo.ShowSeason");
            DropTable("dbo.ShowActivity");

            Sql("UPDATE ActivityCategory SET Name='SERIES' WHERE Name='SHOW'");
        }
    }
}

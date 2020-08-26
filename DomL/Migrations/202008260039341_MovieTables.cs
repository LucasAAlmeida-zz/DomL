namespace DomL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MovieTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MovieActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        MovieId = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .ForeignKey("dbo.Movie", t => t.MovieId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.MovieId);
            
            CreateTable(
                "dbo.Movie",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        DirectorId = c.Int(),
                        SeriesId = c.Int(),
                        NumberInSeries = c.String(),
                        Score = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Person", t => t.DirectorId)
                .ForeignKey("dbo.Series", t => t.SeriesId)
                .Index(t => t.DirectorId)
                .Index(t => t.SeriesId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MovieActivity", "MovieId", "dbo.Movie");
            DropForeignKey("dbo.Movie", "SeriesId", "dbo.Series");
            DropForeignKey("dbo.Movie", "DirectorId", "dbo.Person");
            DropForeignKey("dbo.MovieActivity", "Id", "dbo.Activity");
            DropIndex("dbo.Movie", new[] { "SeriesId" });
            DropIndex("dbo.Movie", new[] { "DirectorId" });
            DropIndex("dbo.MovieActivity", new[] { "MovieId" });
            DropIndex("dbo.MovieActivity", new[] { "Id" });
            DropTable("dbo.Movie");
            DropTable("dbo.MovieActivity");
        }
    }
}

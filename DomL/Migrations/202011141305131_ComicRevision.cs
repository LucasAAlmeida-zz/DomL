namespace DomL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ComicRevision : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ComicVolume", "AuthorId", "dbo.Person");
            DropForeignKey("dbo.ComicVolume", "ScoreId", "dbo.Score");
            DropForeignKey("dbo.ComicVolume", "SeriesId", "dbo.Series");
            DropForeignKey("dbo.ComicVolume", "TypeId", "dbo.MediaType");
            DropForeignKey("dbo.ComicActivity", "ComicVolumeId", "dbo.ComicVolume");
            DropIndex("dbo.ComicActivity", new[] { "ComicVolumeId" });
            DropIndex("dbo.ComicVolume", new[] { "SeriesId" });
            DropIndex("dbo.ComicVolume", new[] { "AuthorId" });
            DropIndex("dbo.ComicVolume", new[] { "TypeId" });
            DropIndex("dbo.ComicVolume", new[] { "ScoreId" });
            CreateTable(
                "dbo.Comic",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        TypeId = c.Int(nullable: false),
                        Author = c.String(),
                        SeriesId = c.Int(),
                        Number = c.String(),
                        Publisher = c.String(),
                        Year = c.Int(nullable: false),
                        Score = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Series", t => t.SeriesId)
                .ForeignKey("dbo.MediaType", t => t.TypeId, cascadeDelete: true)
                .Index(t => t.TypeId)
                .Index(t => t.SeriesId);
            
            AddColumn("dbo.ComicActivity", "ComicId", c => c.Int(nullable: false));
            CreateIndex("dbo.ComicActivity", "ComicId");
            AddForeignKey("dbo.ComicActivity", "ComicId", "dbo.Comic", "Id", cascadeDelete: true);
            DropColumn("dbo.ComicActivity", "ComicVolumeId");
            DropTable("dbo.ComicVolume");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ComicVolume",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SeriesId = c.Int(nullable: false),
                        Chapters = c.String(),
                        AuthorId = c.Int(),
                        TypeId = c.Int(),
                        ScoreId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ComicActivity", "ComicVolumeId", c => c.Int(nullable: false));
            DropForeignKey("dbo.ComicActivity", "ComicId", "dbo.Comic");
            DropForeignKey("dbo.Comic", "TypeId", "dbo.MediaType");
            DropForeignKey("dbo.Comic", "SeriesId", "dbo.Series");
            DropIndex("dbo.Comic", new[] { "SeriesId" });
            DropIndex("dbo.Comic", new[] { "TypeId" });
            DropIndex("dbo.ComicActivity", new[] { "ComicId" });
            DropColumn("dbo.ComicActivity", "ComicId");
            DropTable("dbo.Comic");
            CreateIndex("dbo.ComicVolume", "ScoreId");
            CreateIndex("dbo.ComicVolume", "TypeId");
            CreateIndex("dbo.ComicVolume", "AuthorId");
            CreateIndex("dbo.ComicVolume", "SeriesId");
            CreateIndex("dbo.ComicActivity", "ComicVolumeId");
            AddForeignKey("dbo.ComicActivity", "ComicVolumeId", "dbo.ComicVolume", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ComicVolume", "TypeId", "dbo.MediaType", "Id");
            AddForeignKey("dbo.ComicVolume", "SeriesId", "dbo.Series", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ComicVolume", "ScoreId", "dbo.Score", "Id");
            AddForeignKey("dbo.ComicVolume", "AuthorId", "dbo.Person", "Id");
        }
    }
}

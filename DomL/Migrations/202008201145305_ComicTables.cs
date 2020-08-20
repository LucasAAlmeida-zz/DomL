namespace DomL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ComicTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ComicActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ComicVolumeId = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .ForeignKey("dbo.ComicVolume", t => t.ComicVolumeId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.ComicVolumeId);
            
            CreateTable(
                "dbo.ComicVolume",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SeriesId = c.Int(nullable: false),
                        Chapters = c.String(),
                        AuthorId = c.Int(nullable: false),
                        TypeId = c.Int(nullable: false),
                        Score = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ComicAuthor", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.ComicSeries", t => t.SeriesId, cascadeDelete: true)
                .ForeignKey("dbo.ComicType", t => t.TypeId, cascadeDelete: true)
                .Index(t => t.SeriesId)
                .Index(t => t.AuthorId)
                .Index(t => t.TypeId);
            
            CreateTable(
                "dbo.ComicAuthor",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ComicSeries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ComicType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.Book", "Score", c => c.String());
            AlterColumn("dbo.Book", "NumberInSeries", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ComicActivity", "ComicVolumeId", "dbo.ComicVolume");
            DropForeignKey("dbo.ComicVolume", "TypeId", "dbo.ComicType");
            DropForeignKey("dbo.ComicVolume", "SeriesId", "dbo.ComicSeries");
            DropForeignKey("dbo.ComicVolume", "AuthorId", "dbo.ComicAuthor");
            DropForeignKey("dbo.ComicActivity", "Id", "dbo.Activity");
            DropIndex("dbo.ComicVolume", new[] { "TypeId" });
            DropIndex("dbo.ComicVolume", new[] { "AuthorId" });
            DropIndex("dbo.ComicVolume", new[] { "SeriesId" });
            DropIndex("dbo.ComicActivity", new[] { "ComicVolumeId" });
            DropIndex("dbo.ComicActivity", new[] { "Id" });
            AlterColumn("dbo.Book", "NumberInSeries", c => c.Int(nullable: false));
            AlterColumn("dbo.Book", "Score", c => c.Int(nullable: false));
            DropTable("dbo.ComicType");
            DropTable("dbo.ComicSeries");
            DropTable("dbo.ComicAuthor");
            DropTable("dbo.ComicVolume");
            DropTable("dbo.ComicActivity");
        }
    }
}

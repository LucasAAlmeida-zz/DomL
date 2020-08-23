namespace DomL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activity",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        DayOrder = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        StatusId = c.Int(),
                        PairedActivityId = c.Int(),
                        ActivityBlockId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ActivityBlock", t => t.ActivityBlockId)
                .ForeignKey("dbo.ActivityCategory", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Activity", t => t.PairedActivityId)
                .ForeignKey("dbo.ActivityStatus", t => t.StatusId)
                .Index(t => t.CategoryId)
                .Index(t => t.StatusId)
                .Index(t => t.PairedActivityId)
                .Index(t => t.ActivityBlockId);
            
            CreateTable(
                "dbo.ActivityBlock",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ActivityCategory",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ActivityStatus",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Auto",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AutoActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        AutoId = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .ForeignKey("dbo.Auto", t => t.AutoId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.AutoId);
            
            CreateTable(
                "dbo.Book",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AuthorId = c.Int(nullable: false),
                        SeriesId = c.Int(nullable: false),
                        Title = c.String(),
                        Score = c.String(),
                        NumberInSeries = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Person", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.Series", t => t.SeriesId, cascadeDelete: true)
                .Index(t => t.AuthorId)
                .Index(t => t.SeriesId);
            
            CreateTable(
                "dbo.Person",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Series",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BookActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        BookId = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .ForeignKey("dbo.Book", t => t.BookId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.BookId);
            
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
                .ForeignKey("dbo.Person", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.Series", t => t.SeriesId, cascadeDelete: true)
                .ForeignKey("dbo.MediaType", t => t.TypeId, cascadeDelete: true)
                .Index(t => t.SeriesId)
                .Index(t => t.AuthorId)
                .Index(t => t.TypeId);
            
            CreateTable(
                "dbo.MediaType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DoomActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DoomActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.ComicActivity", "ComicVolumeId", "dbo.ComicVolume");
            DropForeignKey("dbo.ComicVolume", "TypeId", "dbo.MediaType");
            DropForeignKey("dbo.ComicVolume", "SeriesId", "dbo.Series");
            DropForeignKey("dbo.ComicVolume", "AuthorId", "dbo.Person");
            DropForeignKey("dbo.ComicActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.BookActivity", "BookId", "dbo.Book");
            DropForeignKey("dbo.BookActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.Book", "SeriesId", "dbo.Series");
            DropForeignKey("dbo.Book", "AuthorId", "dbo.Person");
            DropForeignKey("dbo.AutoActivity", "AutoId", "dbo.Auto");
            DropForeignKey("dbo.AutoActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.Activity", "StatusId", "dbo.ActivityStatus");
            DropForeignKey("dbo.Activity", "PairedActivityId", "dbo.Activity");
            DropForeignKey("dbo.Activity", "CategoryId", "dbo.ActivityCategory");
            DropForeignKey("dbo.Activity", "ActivityBlockId", "dbo.ActivityBlock");
            DropIndex("dbo.DoomActivity", new[] { "Id" });
            DropIndex("dbo.ComicVolume", new[] { "TypeId" });
            DropIndex("dbo.ComicVolume", new[] { "AuthorId" });
            DropIndex("dbo.ComicVolume", new[] { "SeriesId" });
            DropIndex("dbo.ComicActivity", new[] { "ComicVolumeId" });
            DropIndex("dbo.ComicActivity", new[] { "Id" });
            DropIndex("dbo.BookActivity", new[] { "BookId" });
            DropIndex("dbo.BookActivity", new[] { "Id" });
            DropIndex("dbo.Book", new[] { "SeriesId" });
            DropIndex("dbo.Book", new[] { "AuthorId" });
            DropIndex("dbo.AutoActivity", new[] { "AutoId" });
            DropIndex("dbo.AutoActivity", new[] { "Id" });
            DropIndex("dbo.Activity", new[] { "ActivityBlockId" });
            DropIndex("dbo.Activity", new[] { "PairedActivityId" });
            DropIndex("dbo.Activity", new[] { "StatusId" });
            DropIndex("dbo.Activity", new[] { "CategoryId" });
            DropTable("dbo.DoomActivity");
            DropTable("dbo.MediaType");
            DropTable("dbo.ComicVolume");
            DropTable("dbo.ComicActivity");
            DropTable("dbo.BookActivity");
            DropTable("dbo.Series");
            DropTable("dbo.Person");
            DropTable("dbo.Book");
            DropTable("dbo.AutoActivity");
            DropTable("dbo.Auto");
            DropTable("dbo.ActivityStatus");
            DropTable("dbo.ActivityCategory");
            DropTable("dbo.ActivityBlock");
            DropTable("dbo.Activity");
        }
    }
}

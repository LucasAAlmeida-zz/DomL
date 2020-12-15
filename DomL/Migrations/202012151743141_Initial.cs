namespace DomL.Migrations
{
    using System;
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
                        Block = c.String(),
                        ConsolidatedLine = c.String(),
                        CategoryId = c.Int(nullable: false),
                        StatusId = c.Int(nullable: false),
                        PairedActivityId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Category", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Activity", t => t.PairedActivityId)
                .ForeignKey("dbo.Status", t => t.StatusId, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.StatusId)
                .Index(t => t.PairedActivityId);
            
            CreateTable(
                "dbo.AutoActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Auto = c.String(nullable: false),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .Index(t => t.Id);
            
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
                "dbo.Book",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Person = c.String(),
                        Series = c.String(),
                        Number = c.String(),
                        Company = c.String(),
                        Year = c.String(),
                        Score = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ComicActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ComicId = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .ForeignKey("dbo.Comic", t => t.ComicId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.ComicId);
            
            CreateTable(
                "dbo.Comic",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Type = c.String(),
                        Person = c.String(),
                        Series = c.String(),
                        Number = c.String(),
                        Company = c.String(),
                        Year = c.String(),
                        Score = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CourseActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .ForeignKey("dbo.Course", t => t.CourseId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.Course",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Type = c.String(),
                        Series = c.String(),
                        Number = c.String(),
                        Person = c.String(),
                        Company = c.String(),
                        Year = c.String(),
                        Score = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DoomActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .Index(t => t.Id);
            
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
                        Type = c.String(),
                        Series = c.String(),
                        Number = c.String(),
                        Person = c.String(),
                        Company = c.String(),
                        Year = c.String(),
                        Score = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GiftActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Gift = c.String(nullable: false),
                        IsFrom = c.Boolean(nullable: false),
                        Who = c.String(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.HealthActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Specialty = c.String(),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.MeetActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Person = c.String(nullable: false),
                        Origin = c.String(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .Index(t => t.Id);
            
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
                        Person = c.String(),
                        Company = c.String(),
                        Series = c.String(),
                        Number = c.String(),
                        Year = c.String(),
                        Score = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PetActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Pet = c.String(nullable: false),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.PlayActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Who = c.String(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.PurchaseActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Store = c.String(nullable: false),
                        Product = c.String(nullable: false),
                        Value = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.ShowActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ShowId = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .ForeignKey("dbo.Show", t => t.ShowId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.ShowId);
            
            CreateTable(
                "dbo.Show",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Type = c.String(),
                        Series = c.String(),
                        Number = c.String(),
                        Person = c.String(),
                        Company = c.String(),
                        Year = c.String(),
                        Score = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TravelActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Transport = c.String(nullable: false),
                        Origin = c.String(nullable: false),
                        Destination = c.String(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.WorkActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Work = c.String(nullable: false),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.TravelActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.Activity", "StatusId", "dbo.Status");
            DropForeignKey("dbo.ShowActivity", "ShowId", "dbo.Show");
            DropForeignKey("dbo.ShowActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.PurchaseActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.PlayActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.PetActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.Activity", "PairedActivityId", "dbo.Activity");
            DropForeignKey("dbo.MovieActivity", "MovieId", "dbo.Movie");
            DropForeignKey("dbo.MovieActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.MeetActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.HealthActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.GiftActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.GameActivity", "GameId", "dbo.Game");
            DropForeignKey("dbo.GameActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.EventActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.DoomActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.CourseActivity", "CourseId", "dbo.Course");
            DropForeignKey("dbo.CourseActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.ComicActivity", "ComicId", "dbo.Comic");
            DropForeignKey("dbo.ComicActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.Activity", "CategoryId", "dbo.Category");
            DropForeignKey("dbo.BookActivity", "BookId", "dbo.Book");
            DropForeignKey("dbo.BookActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.AutoActivity", "Id", "dbo.Activity");
            DropIndex("dbo.WorkActivity", new[] { "Id" });
            DropIndex("dbo.TravelActivity", new[] { "Id" });
            DropIndex("dbo.ShowActivity", new[] { "ShowId" });
            DropIndex("dbo.ShowActivity", new[] { "Id" });
            DropIndex("dbo.PurchaseActivity", new[] { "Id" });
            DropIndex("dbo.PlayActivity", new[] { "Id" });
            DropIndex("dbo.PetActivity", new[] { "Id" });
            DropIndex("dbo.MovieActivity", new[] { "MovieId" });
            DropIndex("dbo.MovieActivity", new[] { "Id" });
            DropIndex("dbo.MeetActivity", new[] { "Id" });
            DropIndex("dbo.HealthActivity", new[] { "Id" });
            DropIndex("dbo.GiftActivity", new[] { "Id" });
            DropIndex("dbo.GameActivity", new[] { "GameId" });
            DropIndex("dbo.GameActivity", new[] { "Id" });
            DropIndex("dbo.EventActivity", new[] { "Id" });
            DropIndex("dbo.DoomActivity", new[] { "Id" });
            DropIndex("dbo.CourseActivity", new[] { "CourseId" });
            DropIndex("dbo.CourseActivity", new[] { "Id" });
            DropIndex("dbo.ComicActivity", new[] { "ComicId" });
            DropIndex("dbo.ComicActivity", new[] { "Id" });
            DropIndex("dbo.BookActivity", new[] { "BookId" });
            DropIndex("dbo.BookActivity", new[] { "Id" });
            DropIndex("dbo.AutoActivity", new[] { "Id" });
            DropIndex("dbo.Activity", new[] { "PairedActivityId" });
            DropIndex("dbo.Activity", new[] { "StatusId" });
            DropIndex("dbo.Activity", new[] { "CategoryId" });
            DropTable("dbo.WorkActivity");
            DropTable("dbo.TravelActivity");
            DropTable("dbo.Status");
            DropTable("dbo.Show");
            DropTable("dbo.ShowActivity");
            DropTable("dbo.PurchaseActivity");
            DropTable("dbo.PlayActivity");
            DropTable("dbo.PetActivity");
            DropTable("dbo.Movie");
            DropTable("dbo.MovieActivity");
            DropTable("dbo.MeetActivity");
            DropTable("dbo.HealthActivity");
            DropTable("dbo.GiftActivity");
            DropTable("dbo.Game");
            DropTable("dbo.GameActivity");
            DropTable("dbo.EventActivity");
            DropTable("dbo.DoomActivity");
            DropTable("dbo.Course");
            DropTable("dbo.CourseActivity");
            DropTable("dbo.Comic");
            DropTable("dbo.ComicActivity");
            DropTable("dbo.Category");
            DropTable("dbo.Book");
            DropTable("dbo.BookActivity");
            DropTable("dbo.AutoActivity");
            DropTable("dbo.Activity");
        }
    }
}

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
                        Category = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        DayOrder = c.Int(nullable: false),
                        Classification = c.Int(nullable: false),
                        ActivityBlockId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ActivityBlock", t => t.ActivityBlockId)
                .Index(t => t.ActivityBlockId);
            
            CreateTable(
                "dbo.ActivityBlock",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Month = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
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
                        Score = c.Int(nullable: false),
                        NumberInSeries = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BookAuthor", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.BookSeries", t => t.SeriesId, cascadeDelete: true)
                .Index(t => t.AuthorId)
                .Index(t => t.SeriesId);
            
            CreateTable(
                "dbo.BookAuthor",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BookSeries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BookActivity", "BookId", "dbo.Book");
            DropForeignKey("dbo.BookActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.Book", "SeriesId", "dbo.BookSeries");
            DropForeignKey("dbo.Book", "AuthorId", "dbo.BookAuthor");
            DropForeignKey("dbo.AutoActivity", "AutoId", "dbo.Auto");
            DropForeignKey("dbo.AutoActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.Activity", "ActivityBlockId", "dbo.ActivityBlock");
            DropIndex("dbo.BookActivity", new[] { "BookId" });
            DropIndex("dbo.BookActivity", new[] { "Id" });
            DropIndex("dbo.Book", new[] { "SeriesId" });
            DropIndex("dbo.Book", new[] { "AuthorId" });
            DropIndex("dbo.AutoActivity", new[] { "AutoId" });
            DropIndex("dbo.AutoActivity", new[] { "Id" });
            DropIndex("dbo.Activity", new[] { "ActivityBlockId" });
            DropTable("dbo.BookActivity");
            DropTable("dbo.BookSeries");
            DropTable("dbo.BookAuthor");
            DropTable("dbo.Book");
            DropTable("dbo.AutoActivity");
            DropTable("dbo.Auto");
            DropTable("dbo.ActivityBlock");
            DropTable("dbo.Activity");
        }
    }
}

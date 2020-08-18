namespace DomL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BookTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Book",
                c => new
                    {
                        BookId = c.Int(nullable: false, identity: true),
                        AuthorId = c.Int(nullable: false),
                        SeriesId = c.Int(nullable: false),
                        Title = c.String(),
                        Score = c.Int(nullable: false),
                        NumberInSeries = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookId)
                .ForeignKey("dbo.BookAuthor", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.BookSeries", t => t.SeriesId, cascadeDelete: true)
                .Index(t => t.AuthorId)
                .Index(t => t.SeriesId);
            
            CreateTable(
                "dbo.BookAuthor",
                c => new
                    {
                        AuthorId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.AuthorId);
            
            CreateTable(
                "dbo.BookSeries",
                c => new
                    {
                        SeriesId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.SeriesId);
            
            CreateTable(
                "dbo.BookActivities",
                c => new
                    {
                        ActivityId = c.Int(nullable: false),
                        BookId = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ActivityId)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Book", t => t.BookId, cascadeDelete: true)
                .Index(t => t.ActivityId)
                .Index(t => t.BookId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BookActivities", "BookId", "dbo.Book");
            DropForeignKey("dbo.BookActivities", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Book", "SeriesId", "dbo.BookSeries");
            DropForeignKey("dbo.Book", "AuthorId", "dbo.BookAuthor");
            DropIndex("dbo.BookActivities", new[] { "BookId" });
            DropIndex("dbo.BookActivities", new[] { "ActivityId" });
            DropIndex("dbo.Book", new[] { "SeriesId" });
            DropIndex("dbo.Book", new[] { "AuthorId" });
            DropTable("dbo.BookActivities");
            DropTable("dbo.BookSeries");
            DropTable("dbo.BookAuthor");
            DropTable("dbo.Book");
        }
    }
}

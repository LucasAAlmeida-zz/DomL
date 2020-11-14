namespace DomL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BookRevision : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Book", "AuthorId", "dbo.Person");
            DropForeignKey("dbo.Book", "ScoreId", "dbo.Score");
            DropIndex("dbo.Book", new[] { "AuthorId" });
            DropIndex("dbo.Book", new[] { "ScoreId" });
            AddColumn("dbo.Book", "Author", c => c.String());
            AddColumn("dbo.Book", "Publisher", c => c.String());
            AddColumn("dbo.Book", "Number", c => c.String());
            AddColumn("dbo.Book", "Score", c => c.String());
            AddColumn("dbo.Book", "Year", c => c.Int(nullable: false));
            DropColumn("dbo.Book", "AuthorId");
            DropColumn("dbo.Book", "NumberInSeries");
            DropColumn("dbo.Book", "ScoreId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Book", "ScoreId", c => c.Int());
            AddColumn("dbo.Book", "NumberInSeries", c => c.String());
            AddColumn("dbo.Book", "AuthorId", c => c.Int());
            DropColumn("dbo.Book", "Year");
            DropColumn("dbo.Book", "Score");
            DropColumn("dbo.Book", "Number");
            DropColumn("dbo.Book", "Publisher");
            DropColumn("dbo.Book", "Author");
            CreateIndex("dbo.Book", "ScoreId");
            CreateIndex("dbo.Book", "AuthorId");
            AddForeignKey("dbo.Book", "ScoreId", "dbo.Score", "Id");
            AddForeignKey("dbo.Book", "AuthorId", "dbo.Person", "Id");
        }
    }
}

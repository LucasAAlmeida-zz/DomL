namespace DomL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class NullableColumns : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Book", "AuthorId", "dbo.Person");
            DropForeignKey("dbo.Book", "SeriesId", "dbo.Series");
            DropForeignKey("dbo.ComicVolume", "AuthorId", "dbo.Person");
            DropForeignKey("dbo.ComicVolume", "TypeId", "dbo.MediaType");
            DropIndex("dbo.Book", new[] { "AuthorId" });
            DropIndex("dbo.Book", new[] { "SeriesId" });
            DropIndex("dbo.ComicVolume", new[] { "AuthorId" });
            DropIndex("dbo.ComicVolume", new[] { "TypeId" });
            AlterColumn("dbo.Book", "AuthorId", c => c.Int());
            AlterColumn("dbo.Book", "SeriesId", c => c.Int());
            AlterColumn("dbo.ComicVolume", "AuthorId", c => c.Int());
            AlterColumn("dbo.ComicVolume", "TypeId", c => c.Int());
            CreateIndex("dbo.Book", "AuthorId");
            CreateIndex("dbo.Book", "SeriesId");
            CreateIndex("dbo.ComicVolume", "AuthorId");
            CreateIndex("dbo.ComicVolume", "TypeId");
            AddForeignKey("dbo.Book", "AuthorId", "dbo.Person", "Id");
            AddForeignKey("dbo.Book", "SeriesId", "dbo.Series", "Id");
            AddForeignKey("dbo.ComicVolume", "AuthorId", "dbo.Person", "Id");
            AddForeignKey("dbo.ComicVolume", "TypeId", "dbo.MediaType", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ComicVolume", "TypeId", "dbo.MediaType");
            DropForeignKey("dbo.ComicVolume", "AuthorId", "dbo.Person");
            DropForeignKey("dbo.Book", "SeriesId", "dbo.Series");
            DropForeignKey("dbo.Book", "AuthorId", "dbo.Person");
            DropIndex("dbo.ComicVolume", new[] { "TypeId" });
            DropIndex("dbo.ComicVolume", new[] { "AuthorId" });
            DropIndex("dbo.Book", new[] { "SeriesId" });
            DropIndex("dbo.Book", new[] { "AuthorId" });
            AlterColumn("dbo.ComicVolume", "TypeId", c => c.Int(nullable: false));
            AlterColumn("dbo.ComicVolume", "AuthorId", c => c.Int(nullable: false));
            AlterColumn("dbo.Book", "SeriesId", c => c.Int(nullable: false));
            AlterColumn("dbo.Book", "AuthorId", c => c.Int(nullable: false));
            CreateIndex("dbo.ComicVolume", "TypeId");
            CreateIndex("dbo.ComicVolume", "AuthorId");
            CreateIndex("dbo.Book", "SeriesId");
            CreateIndex("dbo.Book", "AuthorId");
            AddForeignKey("dbo.ComicVolume", "TypeId", "dbo.MediaType", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ComicVolume", "AuthorId", "dbo.Person", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Book", "SeriesId", "dbo.Series", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Book", "AuthorId", "dbo.Person", "Id", cascadeDelete: true);
        }
    }
}

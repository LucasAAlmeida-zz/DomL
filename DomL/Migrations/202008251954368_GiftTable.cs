namespace DomL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GiftTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GiftActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Gift = c.String(nullable: false),
                        IsFrom = c.Boolean(nullable: false),
                        PersonId = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .ForeignKey("dbo.Person", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.PersonId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GiftActivity", "PersonId", "dbo.Person");
            DropForeignKey("dbo.GiftActivity", "Id", "dbo.Activity");
            DropIndex("dbo.GiftActivity", new[] { "PersonId" });
            DropIndex("dbo.GiftActivity", new[] { "Id" });
            DropTable("dbo.GiftActivity");
        }
    }
}

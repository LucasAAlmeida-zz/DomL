namespace DomL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PurchaseTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PurchaseActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        StoreId = c.Int(nullable: false),
                        Product = c.String(nullable: false),
                        Value = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .ForeignKey("dbo.Company", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.StoreId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PurchaseActivity", "StoreId", "dbo.Company");
            DropForeignKey("dbo.PurchaseActivity", "Id", "dbo.Activity");
            DropIndex("dbo.PurchaseActivity", new[] { "StoreId" });
            DropIndex("dbo.PurchaseActivity", new[] { "Id" });
            DropTable("dbo.PurchaseActivity");
        }
    }
}

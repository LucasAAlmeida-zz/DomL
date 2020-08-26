namespace DomL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PetTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PetActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        PetId = c.Int(nullable: false),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .ForeignKey("dbo.Pet", t => t.PetId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.PetId);
            
            CreateTable(
                "dbo.Pet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PetActivity", "PetId", "dbo.Pet");
            DropForeignKey("dbo.PetActivity", "Id", "dbo.Activity");
            DropIndex("dbo.PetActivity", new[] { "PetId" });
            DropIndex("dbo.PetActivity", new[] { "Id" });
            DropTable("dbo.Pet");
            DropTable("dbo.PetActivity");
        }
    }
}

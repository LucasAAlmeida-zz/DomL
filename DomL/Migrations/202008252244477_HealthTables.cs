namespace DomL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HealthTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HealthActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        SpecialtyId = c.Int(),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .ForeignKey("dbo.MedicalSpecialty", t => t.SpecialtyId)
                .Index(t => t.Id)
                .Index(t => t.SpecialtyId);
            
            CreateTable(
                "dbo.MedicalSpecialty",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HealthActivity", "SpecialtyId", "dbo.MedicalSpecialty");
            DropForeignKey("dbo.HealthActivity", "Id", "dbo.Activity");
            DropIndex("dbo.HealthActivity", new[] { "SpecialtyId" });
            DropIndex("dbo.HealthActivity", new[] { "Id" });
            DropTable("dbo.MedicalSpecialty");
            DropTable("dbo.HealthActivity");
        }
    }
}

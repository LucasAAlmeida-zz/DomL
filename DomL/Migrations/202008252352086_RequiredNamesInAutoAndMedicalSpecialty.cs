namespace DomL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequiredNamesInAutoAndMedicalSpecialty : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Auto", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.MedicalSpecialty", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MedicalSpecialty", "Name", c => c.String());
            AlterColumn("dbo.Auto", "Name", c => c.String());
        }
    }
}

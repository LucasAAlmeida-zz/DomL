namespace DomL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class MandatoryDescriptionInDoomAndAuto : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AutoActivity", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.DoomActivity", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DoomActivity", "Description", c => c.String());
            AlterColumn("dbo.AutoActivity", "Description", c => c.String());
        }
    }
}

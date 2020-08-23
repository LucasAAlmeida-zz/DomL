namespace DomL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddOriginalLineToActivityTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activity", "OriginalLine", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Activity", "OriginalLine");
        }
    }
}

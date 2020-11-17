namespace DomL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AutoNameChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AutoActivity", "Auto", c => c.String(nullable: false));
            DropColumn("dbo.AutoActivity", "AutoName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AutoActivity", "AutoName", c => c.String(nullable: false));
            DropColumn("dbo.AutoActivity", "Auto");
        }
    }
}

namespace DomL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AutoNameIsString : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AutoActivity", "AutoId", "dbo.Transport");
            DropIndex("dbo.AutoActivity", new[] { "AutoId" });
            AddColumn("dbo.AutoActivity", "AutoName", c => c.String(nullable: false));
            DropColumn("dbo.AutoActivity", "AutoId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AutoActivity", "AutoId", c => c.Int(nullable: false));
            DropColumn("dbo.AutoActivity", "AutoName");
            CreateIndex("dbo.AutoActivity", "AutoId");
            AddForeignKey("dbo.AutoActivity", "AutoId", "dbo.Transport", "Id", cascadeDelete: true);
        }
    }
}

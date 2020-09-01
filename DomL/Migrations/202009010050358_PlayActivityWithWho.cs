namespace DomL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayActivityWithWho : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PlayActivity", "PersonId", "dbo.Person");
            DropIndex("dbo.PlayActivity", new[] { "PersonId" });
            AddColumn("dbo.PlayActivity", "Who", c => c.String());
            DropColumn("dbo.PlayActivity", "PersonId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PlayActivity", "PersonId", c => c.Int(nullable: false));
            DropColumn("dbo.PlayActivity", "Who");
            CreateIndex("dbo.PlayActivity", "PersonId");
            AddForeignKey("dbo.PlayActivity", "PersonId", "dbo.Person", "Id", cascadeDelete: true);
        }
    }
}

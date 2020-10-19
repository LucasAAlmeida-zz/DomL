namespace DomL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class GiftWithWho : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GiftActivity", "PersonId", "dbo.Person");
            DropIndex("dbo.GiftActivity", new[] { "PersonId" });
            AddColumn("dbo.GiftActivity", "Who", c => c.String(nullable: false));
            AlterColumn("dbo.PlayActivity", "Who", c => c.String(nullable: false));
            DropColumn("dbo.GiftActivity", "PersonId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GiftActivity", "PersonId", c => c.Int(nullable: false));
            AlterColumn("dbo.PlayActivity", "Who", c => c.String());
            DropColumn("dbo.GiftActivity", "Who");
            CreateIndex("dbo.GiftActivity", "PersonId");
            AddForeignKey("dbo.GiftActivity", "PersonId", "dbo.Person", "Id", cascadeDelete: true);
        }
    }
}

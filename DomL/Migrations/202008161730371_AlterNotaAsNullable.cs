namespace DomL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNotaAsNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Book", "Nota", c => c.Int());
            AlterColumn("dbo.Comic", "Nota", c => c.Int());
            AlterColumn("dbo.Game", "Nota", c => c.Int());
            AlterColumn("dbo.Series", "Nota", c => c.Int());
            AlterColumn("dbo.Watch", "Nota", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Watch", "Nota", c => c.Int(nullable: false));
            AlterColumn("dbo.Series", "Nota", c => c.Int(nullable: false));
            AlterColumn("dbo.Game", "Nota", c => c.Int(nullable: false));
            AlterColumn("dbo.Comic", "Nota", c => c.Int(nullable: false));
            AlterColumn("dbo.Book", "Nota", c => c.Int(nullable: false));
        }
    }
}

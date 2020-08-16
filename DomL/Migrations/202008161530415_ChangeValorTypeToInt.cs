namespace DomL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeValorTypeToInt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Purchase", "Valor", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Purchase", "Valor", c => c.String(nullable: false));
        }
    }
}

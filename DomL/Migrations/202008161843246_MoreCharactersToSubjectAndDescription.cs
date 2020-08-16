namespace DomL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreCharactersToSubjectAndDescription : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Book", "Subject", c => c.String(maxLength: 255));
            AlterColumn("dbo.Book", "Description", c => c.String(maxLength: 1000));
            AlterColumn("dbo.Auto", "Subject", c => c.String(maxLength: 255));
            AlterColumn("dbo.Auto", "Description", c => c.String(maxLength: 1000));
            AlterColumn("dbo.Comic", "Subject", c => c.String(maxLength: 255));
            AlterColumn("dbo.Comic", "Description", c => c.String(maxLength: 1000));
            AlterColumn("dbo.Doom", "Subject", c => c.String(maxLength: 255));
            AlterColumn("dbo.Doom", "Description", c => c.String(maxLength: 1000));
            AlterColumn("dbo.Event", "Description", c => c.String(maxLength: 1000));
            AlterColumn("dbo.Game", "Subject", c => c.String(maxLength: 255));
            AlterColumn("dbo.Game", "Description", c => c.String(maxLength: 1000));
            AlterColumn("dbo.Gift", "Subject", c => c.String(maxLength: 255));
            AlterColumn("dbo.Gift", "Description", c => c.String(maxLength: 1000));
            AlterColumn("dbo.Health", "Subject", c => c.String(maxLength: 255));
            AlterColumn("dbo.Health", "Description", c => c.String(maxLength: 1000));
            AlterColumn("dbo.Movie", "Subject", c => c.String(maxLength: 255));
            AlterColumn("dbo.Movie", "Description", c => c.String(maxLength: 1000));
            AlterColumn("dbo.Person", "Subject", c => c.String(maxLength: 255));
            AlterColumn("dbo.Person", "Description", c => c.String(maxLength: 1000));
            AlterColumn("dbo.Pet", "Subject", c => c.String(maxLength: 255));
            AlterColumn("dbo.Pet", "Description", c => c.String(maxLength: 1000));
            AlterColumn("dbo.Play", "Subject", c => c.String(maxLength: 255));
            AlterColumn("dbo.Play", "Description", c => c.String(maxLength: 1000));
            AlterColumn("dbo.Purchase", "Subject", c => c.String(maxLength: 255));
            AlterColumn("dbo.Purchase", "Description", c => c.String(maxLength: 1000));
            AlterColumn("dbo.Series", "Subject", c => c.String(maxLength: 255));
            AlterColumn("dbo.Series", "Description", c => c.String(maxLength: 1000));
            AlterColumn("dbo.Travel", "Subject", c => c.String(maxLength: 255));
            AlterColumn("dbo.Travel", "Description", c => c.String(maxLength: 1000));
            AlterColumn("dbo.Watch", "Subject", c => c.String(maxLength: 255));
            AlterColumn("dbo.Watch", "Description", c => c.String(maxLength: 1000));
            AlterColumn("dbo.Work", "Subject", c => c.String(maxLength: 255));
            AlterColumn("dbo.Work", "Description", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Work", "Description", c => c.String(maxLength: 255));
            AlterColumn("dbo.Work", "Subject", c => c.String(maxLength: 50));
            AlterColumn("dbo.Watch", "Description", c => c.String(maxLength: 255));
            AlterColumn("dbo.Watch", "Subject", c => c.String(maxLength: 50));
            AlterColumn("dbo.Travel", "Description", c => c.String(maxLength: 255));
            AlterColumn("dbo.Travel", "Subject", c => c.String(maxLength: 50));
            AlterColumn("dbo.Series", "Description", c => c.String(maxLength: 255));
            AlterColumn("dbo.Series", "Subject", c => c.String(maxLength: 50));
            AlterColumn("dbo.Purchase", "Description", c => c.String(maxLength: 255));
            AlterColumn("dbo.Purchase", "Subject", c => c.String(maxLength: 50));
            AlterColumn("dbo.Play", "Description", c => c.String(maxLength: 255));
            AlterColumn("dbo.Play", "Subject", c => c.String(maxLength: 50));
            AlterColumn("dbo.Pet", "Description", c => c.String(maxLength: 255));
            AlterColumn("dbo.Pet", "Subject", c => c.String(maxLength: 50));
            AlterColumn("dbo.Person", "Description", c => c.String(maxLength: 255));
            AlterColumn("dbo.Person", "Subject", c => c.String(maxLength: 50));
            AlterColumn("dbo.Movie", "Description", c => c.String(maxLength: 255));
            AlterColumn("dbo.Movie", "Subject", c => c.String(maxLength: 50));
            AlterColumn("dbo.Health", "Description", c => c.String(maxLength: 255));
            AlterColumn("dbo.Health", "Subject", c => c.String(maxLength: 50));
            AlterColumn("dbo.Gift", "Description", c => c.String(maxLength: 255));
            AlterColumn("dbo.Gift", "Subject", c => c.String(maxLength: 50));
            AlterColumn("dbo.Game", "Description", c => c.String(maxLength: 255));
            AlterColumn("dbo.Game", "Subject", c => c.String(maxLength: 50));
            AlterColumn("dbo.Event", "Description", c => c.String(maxLength: 255));
            AlterColumn("dbo.Doom", "Description", c => c.String(maxLength: 255));
            AlterColumn("dbo.Doom", "Subject", c => c.String(maxLength: 50));
            AlterColumn("dbo.Comic", "Description", c => c.String(maxLength: 255));
            AlterColumn("dbo.Comic", "Subject", c => c.String(maxLength: 50));
            AlterColumn("dbo.Auto", "Description", c => c.String(maxLength: 255));
            AlterColumn("dbo.Auto", "Subject", c => c.String(maxLength: 50));
            AlterColumn("dbo.Book", "Description", c => c.String(maxLength: 255));
            AlterColumn("dbo.Book", "Subject", c => c.String(maxLength: 50));
        }
    }
}

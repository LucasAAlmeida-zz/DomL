namespace DomL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddActivityBlockAndEventModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActivityBlock",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Year = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Event",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsImportant = c.Boolean(nullable: false),
                        DayOrder = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Description = c.String(maxLength: 255),
                        ActivityBlockId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ActivityBlock", t => t.ActivityBlockId)
                .Index(t => t.ActivityBlockId);
            
            AddColumn("dbo.Auto", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Auto", "Subject", c => c.String(maxLength: 50));
            AddColumn("dbo.Auto", "Description", c => c.String(maxLength: 255));
            AddColumn("dbo.Auto", "ActivityBlockId", c => c.Int());
            AddColumn("dbo.Book", "Classificacao", c => c.Int(nullable: false));
            AddColumn("dbo.Book", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Book", "Subject", c => c.String(maxLength: 50));
            AddColumn("dbo.Book", "Description", c => c.String(maxLength: 255));
            AddColumn("dbo.Book", "ActivityBlockId", c => c.Int());
            AddColumn("dbo.Comic", "Classificacao", c => c.Int(nullable: false));
            AddColumn("dbo.Comic", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Comic", "Subject", c => c.String(maxLength: 50));
            AddColumn("dbo.Comic", "Description", c => c.String(maxLength: 255));
            AddColumn("dbo.Comic", "ActivityBlockId", c => c.Int());
            AddColumn("dbo.Doom", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Doom", "Subject", c => c.String(maxLength: 50));
            AddColumn("dbo.Doom", "Description", c => c.String(maxLength: 255));
            AddColumn("dbo.Doom", "ActivityBlockId", c => c.Int());
            AddColumn("dbo.Game", "Classificacao", c => c.Int(nullable: false));
            AddColumn("dbo.Game", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Game", "Subject", c => c.String(maxLength: 50));
            AddColumn("dbo.Game", "Description", c => c.String(maxLength: 255));
            AddColumn("dbo.Game", "ActivityBlockId", c => c.Int());
            AddColumn("dbo.Gift", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Gift", "Subject", c => c.String(maxLength: 50));
            AddColumn("dbo.Gift", "Description", c => c.String(maxLength: 255));
            AddColumn("dbo.Gift", "ActivityBlockId", c => c.Int());
            AddColumn("dbo.Health", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Health", "Subject", c => c.String(maxLength: 50));
            AddColumn("dbo.Health", "Description", c => c.String(maxLength: 255));
            AddColumn("dbo.Health", "ActivityBlockId", c => c.Int());
            AddColumn("dbo.Movie", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Movie", "Subject", c => c.String(maxLength: 50));
            AddColumn("dbo.Movie", "Description", c => c.String(maxLength: 255));
            AddColumn("dbo.Movie", "ActivityBlockId", c => c.Int());
            AddColumn("dbo.Person", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Person", "Subject", c => c.String(maxLength: 50));
            AddColumn("dbo.Person", "Description", c => c.String(maxLength: 255));
            AddColumn("dbo.Person", "ActivityBlockId", c => c.Int());
            AddColumn("dbo.Pet", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Pet", "Subject", c => c.String(maxLength: 50));
            AddColumn("dbo.Pet", "Description", c => c.String(maxLength: 255));
            AddColumn("dbo.Pet", "ActivityBlockId", c => c.Int());
            AddColumn("dbo.Play", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Play", "Subject", c => c.String(maxLength: 50));
            AddColumn("dbo.Play", "Description", c => c.String(maxLength: 255));
            AddColumn("dbo.Play", "ActivityBlockId", c => c.Int());
            AddColumn("dbo.Purchase", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Purchase", "Subject", c => c.String(maxLength: 50));
            AddColumn("dbo.Purchase", "Description", c => c.String(maxLength: 255));
            AddColumn("dbo.Purchase", "ActivityBlockId", c => c.Int());
            AddColumn("dbo.Series", "Classificacao", c => c.Int(nullable: false));
            AddColumn("dbo.Series", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Series", "Subject", c => c.String(maxLength: 50));
            AddColumn("dbo.Series", "Description", c => c.String(maxLength: 255));
            AddColumn("dbo.Series", "ActivityBlockId", c => c.Int());
            AddColumn("dbo.Travel", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Travel", "Subject", c => c.String(maxLength: 50));
            AddColumn("dbo.Travel", "Description", c => c.String(maxLength: 255));
            AddColumn("dbo.Travel", "ActivityBlockId", c => c.Int());
            AddColumn("dbo.Watch", "Classificacao", c => c.Int(nullable: false));
            AddColumn("dbo.Watch", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Watch", "Subject", c => c.String(maxLength: 50));
            AddColumn("dbo.Watch", "Description", c => c.String(maxLength: 255));
            AddColumn("dbo.Watch", "ActivityBlockId", c => c.Int());
            AddColumn("dbo.Work", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Work", "Subject", c => c.String(maxLength: 50));
            AddColumn("dbo.Work", "Description", c => c.String(maxLength: 255));
            AddColumn("dbo.Work", "ActivityBlockId", c => c.Int());
            CreateIndex("dbo.Book", "ActivityBlockId");
            CreateIndex("dbo.Auto", "ActivityBlockId");
            CreateIndex("dbo.Comic", "ActivityBlockId");
            CreateIndex("dbo.Doom", "ActivityBlockId");
            CreateIndex("dbo.Game", "ActivityBlockId");
            CreateIndex("dbo.Gift", "ActivityBlockId");
            CreateIndex("dbo.Health", "ActivityBlockId");
            CreateIndex("dbo.Movie", "ActivityBlockId");
            CreateIndex("dbo.Person", "ActivityBlockId");
            CreateIndex("dbo.Pet", "ActivityBlockId");
            CreateIndex("dbo.Play", "ActivityBlockId");
            CreateIndex("dbo.Purchase", "ActivityBlockId");
            CreateIndex("dbo.Series", "ActivityBlockId");
            CreateIndex("dbo.Travel", "ActivityBlockId");
            CreateIndex("dbo.Watch", "ActivityBlockId");
            CreateIndex("dbo.Work", "ActivityBlockId");
            AddForeignKey("dbo.Book", "ActivityBlockId", "dbo.ActivityBlock", "Id");
            AddForeignKey("dbo.Auto", "ActivityBlockId", "dbo.ActivityBlock", "Id");
            AddForeignKey("dbo.Comic", "ActivityBlockId", "dbo.ActivityBlock", "Id");
            AddForeignKey("dbo.Doom", "ActivityBlockId", "dbo.ActivityBlock", "Id");
            AddForeignKey("dbo.Game", "ActivityBlockId", "dbo.ActivityBlock", "Id");
            AddForeignKey("dbo.Gift", "ActivityBlockId", "dbo.ActivityBlock", "Id");
            AddForeignKey("dbo.Health", "ActivityBlockId", "dbo.ActivityBlock", "Id");
            AddForeignKey("dbo.Movie", "ActivityBlockId", "dbo.ActivityBlock", "Id");
            AddForeignKey("dbo.Person", "ActivityBlockId", "dbo.ActivityBlock", "Id");
            AddForeignKey("dbo.Pet", "ActivityBlockId", "dbo.ActivityBlock", "Id");
            AddForeignKey("dbo.Play", "ActivityBlockId", "dbo.ActivityBlock", "Id");
            AddForeignKey("dbo.Purchase", "ActivityBlockId", "dbo.ActivityBlock", "Id");
            AddForeignKey("dbo.Series", "ActivityBlockId", "dbo.ActivityBlock", "Id");
            AddForeignKey("dbo.Travel", "ActivityBlockId", "dbo.ActivityBlock", "Id");
            AddForeignKey("dbo.Watch", "ActivityBlockId", "dbo.ActivityBlock", "Id");
            AddForeignKey("dbo.Work", "ActivityBlockId", "dbo.ActivityBlock", "Id");
            DropColumn("dbo.Auto", "Dia");
            DropColumn("dbo.Auto", "Assunto");
            DropColumn("dbo.Auto", "Descricao");
            DropColumn("dbo.Book", "Dia");
            DropColumn("dbo.Book", "DiaTermino");
            DropColumn("dbo.Book", "Assunto");
            DropColumn("dbo.Book", "Descricao");
            DropColumn("dbo.Comic", "Dia");
            DropColumn("dbo.Comic", "DiaTermino");
            DropColumn("dbo.Comic", "Assunto");
            DropColumn("dbo.Comic", "Descricao");
            DropColumn("dbo.Doom", "Dia");
            DropColumn("dbo.Doom", "Assunto");
            DropColumn("dbo.Doom", "Descricao");
            DropColumn("dbo.Game", "Dia");
            DropColumn("dbo.Game", "DiaTermino");
            DropColumn("dbo.Game", "Assunto");
            DropColumn("dbo.Game", "Descricao");
            DropColumn("dbo.Gift", "Dia");
            DropColumn("dbo.Gift", "Assunto");
            DropColumn("dbo.Gift", "Descricao");
            DropColumn("dbo.Health", "Dia");
            DropColumn("dbo.Health", "Assunto");
            DropColumn("dbo.Health", "Descricao");
            DropColumn("dbo.Movie", "Dia");
            DropColumn("dbo.Movie", "Assunto");
            DropColumn("dbo.Movie", "Descricao");
            DropColumn("dbo.Person", "Dia");
            DropColumn("dbo.Person", "Assunto");
            DropColumn("dbo.Person", "Descricao");
            DropColumn("dbo.Pet", "Dia");
            DropColumn("dbo.Pet", "Assunto");
            DropColumn("dbo.Pet", "Descricao");
            DropColumn("dbo.Play", "Dia");
            DropColumn("dbo.Play", "Assunto");
            DropColumn("dbo.Play", "Descricao");
            DropColumn("dbo.Purchase", "Dia");
            DropColumn("dbo.Purchase", "Assunto");
            DropColumn("dbo.Purchase", "Descricao");
            DropColumn("dbo.Series", "Dia");
            DropColumn("dbo.Series", "DiaTermino");
            DropColumn("dbo.Series", "Assunto");
            DropColumn("dbo.Series", "Descricao");
            DropColumn("dbo.Travel", "Dia");
            DropColumn("dbo.Travel", "Assunto");
            DropColumn("dbo.Travel", "Descricao");
            DropColumn("dbo.Watch", "Dia");
            DropColumn("dbo.Watch", "DiaTermino");
            DropColumn("dbo.Watch", "Assunto");
            DropColumn("dbo.Watch", "Descricao");
            DropColumn("dbo.Work", "Dia");
            DropColumn("dbo.Work", "Assunto");
            DropColumn("dbo.Work", "Descricao");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Work", "Descricao", c => c.String(maxLength: 255));
            AddColumn("dbo.Work", "Assunto", c => c.String(maxLength: 50));
            AddColumn("dbo.Work", "Dia", c => c.DateTime(nullable: false));
            AddColumn("dbo.Watch", "Descricao", c => c.String(maxLength: 255));
            AddColumn("dbo.Watch", "Assunto", c => c.String(maxLength: 50));
            AddColumn("dbo.Watch", "DiaTermino", c => c.DateTime());
            AddColumn("dbo.Watch", "Dia", c => c.DateTime());
            AddColumn("dbo.Travel", "Descricao", c => c.String(maxLength: 255));
            AddColumn("dbo.Travel", "Assunto", c => c.String(maxLength: 50));
            AddColumn("dbo.Travel", "Dia", c => c.DateTime(nullable: false));
            AddColumn("dbo.Series", "Descricao", c => c.String(maxLength: 255));
            AddColumn("dbo.Series", "Assunto", c => c.String(maxLength: 50));
            AddColumn("dbo.Series", "DiaTermino", c => c.DateTime());
            AddColumn("dbo.Series", "Dia", c => c.DateTime());
            AddColumn("dbo.Purchase", "Descricao", c => c.String(maxLength: 255));
            AddColumn("dbo.Purchase", "Assunto", c => c.String(maxLength: 50));
            AddColumn("dbo.Purchase", "Dia", c => c.DateTime(nullable: false));
            AddColumn("dbo.Play", "Descricao", c => c.String(maxLength: 255));
            AddColumn("dbo.Play", "Assunto", c => c.String(maxLength: 50));
            AddColumn("dbo.Play", "Dia", c => c.DateTime(nullable: false));
            AddColumn("dbo.Pet", "Descricao", c => c.String(maxLength: 255));
            AddColumn("dbo.Pet", "Assunto", c => c.String(maxLength: 50));
            AddColumn("dbo.Pet", "Dia", c => c.DateTime(nullable: false));
            AddColumn("dbo.Person", "Descricao", c => c.String(maxLength: 255));
            AddColumn("dbo.Person", "Assunto", c => c.String(maxLength: 50));
            AddColumn("dbo.Person", "Dia", c => c.DateTime(nullable: false));
            AddColumn("dbo.Movie", "Descricao", c => c.String(maxLength: 255));
            AddColumn("dbo.Movie", "Assunto", c => c.String(maxLength: 50));
            AddColumn("dbo.Movie", "Dia", c => c.DateTime(nullable: false));
            AddColumn("dbo.Health", "Descricao", c => c.String(maxLength: 255));
            AddColumn("dbo.Health", "Assunto", c => c.String(maxLength: 50));
            AddColumn("dbo.Health", "Dia", c => c.DateTime(nullable: false));
            AddColumn("dbo.Gift", "Descricao", c => c.String(maxLength: 255));
            AddColumn("dbo.Gift", "Assunto", c => c.String(maxLength: 50));
            AddColumn("dbo.Gift", "Dia", c => c.DateTime(nullable: false));
            AddColumn("dbo.Game", "Descricao", c => c.String(maxLength: 255));
            AddColumn("dbo.Game", "Assunto", c => c.String(maxLength: 50));
            AddColumn("dbo.Game", "DiaTermino", c => c.DateTime());
            AddColumn("dbo.Game", "Dia", c => c.DateTime());
            AddColumn("dbo.Doom", "Descricao", c => c.String(maxLength: 255));
            AddColumn("dbo.Doom", "Assunto", c => c.String(maxLength: 50));
            AddColumn("dbo.Doom", "Dia", c => c.DateTime(nullable: false));
            AddColumn("dbo.Comic", "Descricao", c => c.String(maxLength: 255));
            AddColumn("dbo.Comic", "Assunto", c => c.String(maxLength: 50));
            AddColumn("dbo.Comic", "DiaTermino", c => c.DateTime());
            AddColumn("dbo.Comic", "Dia", c => c.DateTime());
            AddColumn("dbo.Book", "Descricao", c => c.String(maxLength: 255));
            AddColumn("dbo.Book", "Assunto", c => c.String(maxLength: 50));
            AddColumn("dbo.Book", "DiaTermino", c => c.DateTime());
            AddColumn("dbo.Book", "Dia", c => c.DateTime());
            AddColumn("dbo.Auto", "Descricao", c => c.String(maxLength: 255));
            AddColumn("dbo.Auto", "Assunto", c => c.String(maxLength: 50));
            AddColumn("dbo.Auto", "Dia", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.Work", "ActivityBlockId", "dbo.ActivityBlock");
            DropForeignKey("dbo.Watch", "ActivityBlockId", "dbo.ActivityBlock");
            DropForeignKey("dbo.Travel", "ActivityBlockId", "dbo.ActivityBlock");
            DropForeignKey("dbo.Series", "ActivityBlockId", "dbo.ActivityBlock");
            DropForeignKey("dbo.Purchase", "ActivityBlockId", "dbo.ActivityBlock");
            DropForeignKey("dbo.Play", "ActivityBlockId", "dbo.ActivityBlock");
            DropForeignKey("dbo.Pet", "ActivityBlockId", "dbo.ActivityBlock");
            DropForeignKey("dbo.Person", "ActivityBlockId", "dbo.ActivityBlock");
            DropForeignKey("dbo.Movie", "ActivityBlockId", "dbo.ActivityBlock");
            DropForeignKey("dbo.Health", "ActivityBlockId", "dbo.ActivityBlock");
            DropForeignKey("dbo.Gift", "ActivityBlockId", "dbo.ActivityBlock");
            DropForeignKey("dbo.Game", "ActivityBlockId", "dbo.ActivityBlock");
            DropForeignKey("dbo.Event", "ActivityBlockId", "dbo.ActivityBlock");
            DropForeignKey("dbo.Doom", "ActivityBlockId", "dbo.ActivityBlock");
            DropForeignKey("dbo.Comic", "ActivityBlockId", "dbo.ActivityBlock");
            DropForeignKey("dbo.Auto", "ActivityBlockId", "dbo.ActivityBlock");
            DropForeignKey("dbo.Book", "ActivityBlockId", "dbo.ActivityBlock");
            DropIndex("dbo.Work", new[] { "ActivityBlockId" });
            DropIndex("dbo.Watch", new[] { "ActivityBlockId" });
            DropIndex("dbo.Travel", new[] { "ActivityBlockId" });
            DropIndex("dbo.Series", new[] { "ActivityBlockId" });
            DropIndex("dbo.Purchase", new[] { "ActivityBlockId" });
            DropIndex("dbo.Play", new[] { "ActivityBlockId" });
            DropIndex("dbo.Pet", new[] { "ActivityBlockId" });
            DropIndex("dbo.Person", new[] { "ActivityBlockId" });
            DropIndex("dbo.Movie", new[] { "ActivityBlockId" });
            DropIndex("dbo.Health", new[] { "ActivityBlockId" });
            DropIndex("dbo.Gift", new[] { "ActivityBlockId" });
            DropIndex("dbo.Game", new[] { "ActivityBlockId" });
            DropIndex("dbo.Event", new[] { "ActivityBlockId" });
            DropIndex("dbo.Doom", new[] { "ActivityBlockId" });
            DropIndex("dbo.Comic", new[] { "ActivityBlockId" });
            DropIndex("dbo.Auto", new[] { "ActivityBlockId" });
            DropIndex("dbo.Book", new[] { "ActivityBlockId" });
            DropColumn("dbo.Work", "ActivityBlockId");
            DropColumn("dbo.Work", "Description");
            DropColumn("dbo.Work", "Subject");
            DropColumn("dbo.Work", "Date");
            DropColumn("dbo.Watch", "ActivityBlockId");
            DropColumn("dbo.Watch", "Description");
            DropColumn("dbo.Watch", "Subject");
            DropColumn("dbo.Watch", "Date");
            DropColumn("dbo.Watch", "Classificacao");
            DropColumn("dbo.Travel", "ActivityBlockId");
            DropColumn("dbo.Travel", "Description");
            DropColumn("dbo.Travel", "Subject");
            DropColumn("dbo.Travel", "Date");
            DropColumn("dbo.Series", "ActivityBlockId");
            DropColumn("dbo.Series", "Description");
            DropColumn("dbo.Series", "Subject");
            DropColumn("dbo.Series", "Date");
            DropColumn("dbo.Series", "Classificacao");
            DropColumn("dbo.Purchase", "ActivityBlockId");
            DropColumn("dbo.Purchase", "Description");
            DropColumn("dbo.Purchase", "Subject");
            DropColumn("dbo.Purchase", "Date");
            DropColumn("dbo.Play", "ActivityBlockId");
            DropColumn("dbo.Play", "Description");
            DropColumn("dbo.Play", "Subject");
            DropColumn("dbo.Play", "Date");
            DropColumn("dbo.Pet", "ActivityBlockId");
            DropColumn("dbo.Pet", "Description");
            DropColumn("dbo.Pet", "Subject");
            DropColumn("dbo.Pet", "Date");
            DropColumn("dbo.Person", "ActivityBlockId");
            DropColumn("dbo.Person", "Description");
            DropColumn("dbo.Person", "Subject");
            DropColumn("dbo.Person", "Date");
            DropColumn("dbo.Movie", "ActivityBlockId");
            DropColumn("dbo.Movie", "Description");
            DropColumn("dbo.Movie", "Subject");
            DropColumn("dbo.Movie", "Date");
            DropColumn("dbo.Health", "ActivityBlockId");
            DropColumn("dbo.Health", "Description");
            DropColumn("dbo.Health", "Subject");
            DropColumn("dbo.Health", "Date");
            DropColumn("dbo.Gift", "ActivityBlockId");
            DropColumn("dbo.Gift", "Description");
            DropColumn("dbo.Gift", "Subject");
            DropColumn("dbo.Gift", "Date");
            DropColumn("dbo.Game", "ActivityBlockId");
            DropColumn("dbo.Game", "Description");
            DropColumn("dbo.Game", "Subject");
            DropColumn("dbo.Game", "Date");
            DropColumn("dbo.Game", "Classificacao");
            DropColumn("dbo.Doom", "ActivityBlockId");
            DropColumn("dbo.Doom", "Description");
            DropColumn("dbo.Doom", "Subject");
            DropColumn("dbo.Doom", "Date");
            DropColumn("dbo.Comic", "ActivityBlockId");
            DropColumn("dbo.Comic", "Description");
            DropColumn("dbo.Comic", "Subject");
            DropColumn("dbo.Comic", "Date");
            DropColumn("dbo.Comic", "Classificacao");
            DropColumn("dbo.Book", "ActivityBlockId");
            DropColumn("dbo.Book", "Description");
            DropColumn("dbo.Book", "Subject");
            DropColumn("dbo.Book", "Date");
            DropColumn("dbo.Book", "Classificacao");
            DropColumn("dbo.Auto", "ActivityBlockId");
            DropColumn("dbo.Auto", "Description");
            DropColumn("dbo.Auto", "Subject");
            DropColumn("dbo.Auto", "Date");
            DropTable("dbo.Event");
            DropTable("dbo.ActivityBlock");
        }
    }
}

namespace DomL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Auto",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DayOrder = c.Int(nullable: false),
                        Dia = c.DateTime(nullable: false),
                        Assunto = c.String(maxLength: 50),
                        Descricao = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Book",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DeQuem = c.String(nullable: false, maxLength: 50),
                        Dia = c.DateTime(),
                        DiaTermino = c.DateTime(),
                        Nota = c.Int(nullable: false),
                        DayOrder = c.Int(nullable: false),
                        Assunto = c.String(maxLength: 50),
                        Descricao = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Comic",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DeQuem = c.String(nullable: false, maxLength: 50),
                        Dia = c.DateTime(),
                        DiaTermino = c.DateTime(),
                        Nota = c.Int(nullable: false),
                        DayOrder = c.Int(nullable: false),
                        Assunto = c.String(maxLength: 50),
                        Descricao = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Doom",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DayOrder = c.Int(nullable: false),
                        Dia = c.DateTime(nullable: false),
                        Assunto = c.String(maxLength: 50),
                        Descricao = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Game",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DeQuem = c.String(nullable: false, maxLength: 50),
                        Dia = c.DateTime(),
                        DiaTermino = c.DateTime(),
                        Nota = c.Int(nullable: false),
                        DayOrder = c.Int(nullable: false),
                        Assunto = c.String(maxLength: 50),
                        Descricao = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Gift",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DeQuem = c.String(nullable: false),
                        DayOrder = c.Int(nullable: false),
                        Dia = c.DateTime(nullable: false),
                        Assunto = c.String(maxLength: 50),
                        Descricao = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Health",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DayOrder = c.Int(nullable: false),
                        Dia = c.DateTime(nullable: false),
                        Assunto = c.String(maxLength: 50),
                        Descricao = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Movie",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nota = c.String(nullable: false),
                        DayOrder = c.Int(nullable: false),
                        Dia = c.DateTime(nullable: false),
                        Assunto = c.String(maxLength: 50),
                        Descricao = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Person",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Origem = c.String(nullable: false),
                        DayOrder = c.Int(nullable: false),
                        Dia = c.DateTime(nullable: false),
                        Assunto = c.String(maxLength: 50),
                        Descricao = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DayOrder = c.Int(nullable: false),
                        Dia = c.DateTime(nullable: false),
                        Assunto = c.String(maxLength: 50),
                        Descricao = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Play",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DayOrder = c.Int(nullable: false),
                        Dia = c.DateTime(nullable: false),
                        Assunto = c.String(maxLength: 50),
                        Descricao = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Purchase",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Loja = c.String(nullable: false),
                        Valor = c.String(nullable: false),
                        DayOrder = c.Int(nullable: false),
                        Dia = c.DateTime(nullable: false),
                        Assunto = c.String(maxLength: 50),
                        Descricao = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Series",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DeQuem = c.String(nullable: false, maxLength: 50),
                        Dia = c.DateTime(),
                        DiaTermino = c.DateTime(),
                        Nota = c.Int(nullable: false),
                        DayOrder = c.Int(nullable: false),
                        Assunto = c.String(maxLength: 50),
                        Descricao = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Travel",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MeioTransporte = c.String(nullable: false),
                        DayOrder = c.Int(nullable: false),
                        Dia = c.DateTime(nullable: false),
                        Assunto = c.String(maxLength: 50),
                        Descricao = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Watch",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DeQuem = c.String(nullable: false, maxLength: 50),
                        Dia = c.DateTime(),
                        DiaTermino = c.DateTime(),
                        Nota = c.Int(nullable: false),
                        DayOrder = c.Int(nullable: false),
                        Assunto = c.String(maxLength: 50),
                        Descricao = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Work",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DayOrder = c.Int(nullable: false),
                        Dia = c.DateTime(nullable: false),
                        Assunto = c.String(maxLength: 50),
                        Descricao = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Work");
            DropTable("dbo.Watch");
            DropTable("dbo.Travel");
            DropTable("dbo.Series");
            DropTable("dbo.Purchase");
            DropTable("dbo.Play");
            DropTable("dbo.Pet");
            DropTable("dbo.Person");
            DropTable("dbo.Movie");
            DropTable("dbo.Health");
            DropTable("dbo.Gift");
            DropTable("dbo.Game");
            DropTable("dbo.Doom");
            DropTable("dbo.Comic");
            DropTable("dbo.Book");
            DropTable("dbo.Auto");
        }
    }
}

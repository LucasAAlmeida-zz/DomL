namespace DomL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activity",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        DayOrder = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        StatusId = c.Int(),
                        PairedActivityId = c.Int(),
                        ActivityBlockId = c.Int(),
                        OriginalLine = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ActivityBlock", t => t.ActivityBlockId)
                .ForeignKey("dbo.ActivityCategory", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Activity", t => t.PairedActivityId)
                .ForeignKey("dbo.ActivityStatus", t => t.StatusId)
                .Index(t => t.CategoryId)
                .Index(t => t.StatusId)
                .Index(t => t.PairedActivityId)
                .Index(t => t.ActivityBlockId);
            
            CreateTable(
                "dbo.ActivityBlock",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AutoActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        AutoId = c.Int(nullable: false),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .ForeignKey("dbo.Transport", t => t.AutoId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.AutoId);
            
            CreateTable(
                "dbo.Transport",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BookActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        BookId = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .ForeignKey("dbo.Book", t => t.BookId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.BookId);
            
            CreateTable(
                "dbo.Book",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        AuthorId = c.Int(),
                        SeriesId = c.Int(),
                        NumberInSeries = c.String(),
                        ScoreId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Person", t => t.AuthorId)
                .ForeignKey("dbo.Score", t => t.ScoreId)
                .ForeignKey("dbo.Series", t => t.SeriesId)
                .Index(t => t.AuthorId)
                .Index(t => t.SeriesId)
                .Index(t => t.ScoreId);
            
            CreateTable(
                "dbo.Person",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Score",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Value = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Series",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        FranchiseId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Franchise", t => t.FranchiseId)
                .Index(t => t.FranchiseId);
            
            CreateTable(
                "dbo.Franchise",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CreatorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Person", t => t.CreatorId)
                .Index(t => t.CreatorId);
            
            CreateTable(
                "dbo.ActivityCategory",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ComicActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ComicVolumeId = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .ForeignKey("dbo.ComicVolume", t => t.ComicVolumeId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.ComicVolumeId);
            
            CreateTable(
                "dbo.ComicVolume",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SeriesId = c.Int(nullable: false),
                        Chapters = c.String(),
                        AuthorId = c.Int(),
                        TypeId = c.Int(),
                        ScoreId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Person", t => t.AuthorId)
                .ForeignKey("dbo.Score", t => t.ScoreId)
                .ForeignKey("dbo.Series", t => t.SeriesId, cascadeDelete: true)
                .ForeignKey("dbo.MediaType", t => t.TypeId)
                .Index(t => t.SeriesId)
                .Index(t => t.AuthorId)
                .Index(t => t.TypeId)
                .Index(t => t.ScoreId);
            
            CreateTable(
                "dbo.MediaType",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ActivityCategory", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.DoomActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.EventActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Description = c.String(nullable: false),
                        IsImportant = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.GameActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        GameId = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .ForeignKey("dbo.Game", t => t.GameId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.GameId);
            
            CreateTable(
                "dbo.Game",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        PlatformId = c.Int(nullable: false),
                        SeriesId = c.Int(),
                        NumberInSeries = c.String(),
                        DirectorId = c.Int(),
                        PublisherId = c.Int(),
                        ScoreId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Person", t => t.DirectorId)
                .ForeignKey("dbo.MediaType", t => t.PlatformId, cascadeDelete: true)
                .ForeignKey("dbo.Company", t => t.PublisherId)
                .ForeignKey("dbo.Score", t => t.ScoreId)
                .ForeignKey("dbo.Series", t => t.SeriesId)
                .Index(t => t.PlatformId)
                .Index(t => t.SeriesId)
                .Index(t => t.DirectorId)
                .Index(t => t.PublisherId)
                .Index(t => t.ScoreId);
            
            CreateTable(
                "dbo.Company",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GiftActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Gift = c.String(nullable: false),
                        IsFrom = c.Boolean(nullable: false),
                        PersonId = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .ForeignKey("dbo.Person", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.PersonId);
            
            CreateTable(
                "dbo.HealthActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        SpecialtyId = c.Int(),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .ForeignKey("dbo.Company", t => t.SpecialtyId)
                .Index(t => t.Id)
                .Index(t => t.SpecialtyId);
            
            CreateTable(
                "dbo.MeetActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        PersonId = c.Int(nullable: false),
                        Origin = c.String(nullable: false),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .ForeignKey("dbo.Person", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.PersonId);
            
            CreateTable(
                "dbo.MovieActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        MovieId = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .ForeignKey("dbo.Movie", t => t.MovieId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.MovieId);
            
            CreateTable(
                "dbo.Movie",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        DirectorId = c.Int(),
                        SeriesId = c.Int(),
                        NumberInSeries = c.String(),
                        ScoreId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Person", t => t.DirectorId)
                .ForeignKey("dbo.Score", t => t.ScoreId)
                .ForeignKey("dbo.Series", t => t.SeriesId)
                .Index(t => t.DirectorId)
                .Index(t => t.SeriesId)
                .Index(t => t.ScoreId);
            
            CreateTable(
                "dbo.PetActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        PetId = c.Int(nullable: false),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .ForeignKey("dbo.Person", t => t.PetId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.PetId);
            
            CreateTable(
                "dbo.PlayActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        PersonId = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .ForeignKey("dbo.Person", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.PersonId);
            
            CreateTable(
                "dbo.PurchaseActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        StoreId = c.Int(nullable: false),
                        Product = c.String(nullable: false),
                        Value = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .ForeignKey("dbo.Company", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.StoreId);
            
            CreateTable(
                "dbo.ShowActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ShowSeasonId = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .ForeignKey("dbo.ShowSeason", t => t.ShowSeasonId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.ShowSeasonId);
            
            CreateTable(
                "dbo.ShowSeason",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SeriesId = c.Int(nullable: false),
                        Season = c.String(),
                        DirectorId = c.Int(),
                        TypeId = c.Int(),
                        ScoreId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Person", t => t.DirectorId)
                .ForeignKey("dbo.Score", t => t.ScoreId)
                .ForeignKey("dbo.Series", t => t.SeriesId, cascadeDelete: true)
                .ForeignKey("dbo.MediaType", t => t.TypeId)
                .Index(t => t.SeriesId)
                .Index(t => t.DirectorId)
                .Index(t => t.TypeId)
                .Index(t => t.ScoreId);
            
            CreateTable(
                "dbo.ActivityStatus",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TravelActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        TransportId = c.Int(nullable: false),
                        OriginId = c.Int(),
                        DestinationId = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .ForeignKey("dbo.Location", t => t.DestinationId, cascadeDelete: true)
                .ForeignKey("dbo.Location", t => t.OriginId)
                .ForeignKey("dbo.Transport", t => t.TransportId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.TransportId)
                .Index(t => t.OriginId)
                .Index(t => t.DestinationId);
            
            CreateTable(
                "dbo.Location",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WorkActivity",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        WorkId = c.Int(nullable: false),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .ForeignKey("dbo.Company", t => t.WorkId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.WorkId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkActivity", "WorkId", "dbo.Company");
            DropForeignKey("dbo.WorkActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.TravelActivity", "TransportId", "dbo.Transport");
            DropForeignKey("dbo.TravelActivity", "OriginId", "dbo.Location");
            DropForeignKey("dbo.TravelActivity", "DestinationId", "dbo.Location");
            DropForeignKey("dbo.TravelActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.Activity", "StatusId", "dbo.ActivityStatus");
            DropForeignKey("dbo.ShowActivity", "ShowSeasonId", "dbo.ShowSeason");
            DropForeignKey("dbo.ShowSeason", "TypeId", "dbo.MediaType");
            DropForeignKey("dbo.ShowSeason", "SeriesId", "dbo.Series");
            DropForeignKey("dbo.ShowSeason", "ScoreId", "dbo.Score");
            DropForeignKey("dbo.ShowSeason", "DirectorId", "dbo.Person");
            DropForeignKey("dbo.ShowActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.PurchaseActivity", "StoreId", "dbo.Company");
            DropForeignKey("dbo.PurchaseActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.PlayActivity", "PersonId", "dbo.Person");
            DropForeignKey("dbo.PlayActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.PetActivity", "PetId", "dbo.Person");
            DropForeignKey("dbo.PetActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.Activity", "PairedActivityId", "dbo.Activity");
            DropForeignKey("dbo.MovieActivity", "MovieId", "dbo.Movie");
            DropForeignKey("dbo.Movie", "SeriesId", "dbo.Series");
            DropForeignKey("dbo.Movie", "ScoreId", "dbo.Score");
            DropForeignKey("dbo.Movie", "DirectorId", "dbo.Person");
            DropForeignKey("dbo.MovieActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.MeetActivity", "PersonId", "dbo.Person");
            DropForeignKey("dbo.MeetActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.HealthActivity", "SpecialtyId", "dbo.Company");
            DropForeignKey("dbo.HealthActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.GiftActivity", "PersonId", "dbo.Person");
            DropForeignKey("dbo.GiftActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.GameActivity", "GameId", "dbo.Game");
            DropForeignKey("dbo.Game", "SeriesId", "dbo.Series");
            DropForeignKey("dbo.Game", "ScoreId", "dbo.Score");
            DropForeignKey("dbo.Game", "PublisherId", "dbo.Company");
            DropForeignKey("dbo.Game", "PlatformId", "dbo.MediaType");
            DropForeignKey("dbo.Game", "DirectorId", "dbo.Person");
            DropForeignKey("dbo.GameActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.EventActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.DoomActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.ComicActivity", "ComicVolumeId", "dbo.ComicVolume");
            DropForeignKey("dbo.ComicVolume", "TypeId", "dbo.MediaType");
            DropForeignKey("dbo.MediaType", "CategoryId", "dbo.ActivityCategory");
            DropForeignKey("dbo.ComicVolume", "SeriesId", "dbo.Series");
            DropForeignKey("dbo.ComicVolume", "ScoreId", "dbo.Score");
            DropForeignKey("dbo.ComicVolume", "AuthorId", "dbo.Person");
            DropForeignKey("dbo.ComicActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.Activity", "CategoryId", "dbo.ActivityCategory");
            DropForeignKey("dbo.BookActivity", "BookId", "dbo.Book");
            DropForeignKey("dbo.Book", "SeriesId", "dbo.Series");
            DropForeignKey("dbo.Series", "FranchiseId", "dbo.Franchise");
            DropForeignKey("dbo.Franchise", "CreatorId", "dbo.Person");
            DropForeignKey("dbo.Book", "ScoreId", "dbo.Score");
            DropForeignKey("dbo.Book", "AuthorId", "dbo.Person");
            DropForeignKey("dbo.BookActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.AutoActivity", "AutoId", "dbo.Transport");
            DropForeignKey("dbo.AutoActivity", "Id", "dbo.Activity");
            DropForeignKey("dbo.Activity", "ActivityBlockId", "dbo.ActivityBlock");
            DropIndex("dbo.WorkActivity", new[] { "WorkId" });
            DropIndex("dbo.WorkActivity", new[] { "Id" });
            DropIndex("dbo.TravelActivity", new[] { "DestinationId" });
            DropIndex("dbo.TravelActivity", new[] { "OriginId" });
            DropIndex("dbo.TravelActivity", new[] { "TransportId" });
            DropIndex("dbo.TravelActivity", new[] { "Id" });
            DropIndex("dbo.ShowSeason", new[] { "ScoreId" });
            DropIndex("dbo.ShowSeason", new[] { "TypeId" });
            DropIndex("dbo.ShowSeason", new[] { "DirectorId" });
            DropIndex("dbo.ShowSeason", new[] { "SeriesId" });
            DropIndex("dbo.ShowActivity", new[] { "ShowSeasonId" });
            DropIndex("dbo.ShowActivity", new[] { "Id" });
            DropIndex("dbo.PurchaseActivity", new[] { "StoreId" });
            DropIndex("dbo.PurchaseActivity", new[] { "Id" });
            DropIndex("dbo.PlayActivity", new[] { "PersonId" });
            DropIndex("dbo.PlayActivity", new[] { "Id" });
            DropIndex("dbo.PetActivity", new[] { "PetId" });
            DropIndex("dbo.PetActivity", new[] { "Id" });
            DropIndex("dbo.Movie", new[] { "ScoreId" });
            DropIndex("dbo.Movie", new[] { "SeriesId" });
            DropIndex("dbo.Movie", new[] { "DirectorId" });
            DropIndex("dbo.MovieActivity", new[] { "MovieId" });
            DropIndex("dbo.MovieActivity", new[] { "Id" });
            DropIndex("dbo.MeetActivity", new[] { "PersonId" });
            DropIndex("dbo.MeetActivity", new[] { "Id" });
            DropIndex("dbo.HealthActivity", new[] { "SpecialtyId" });
            DropIndex("dbo.HealthActivity", new[] { "Id" });
            DropIndex("dbo.GiftActivity", new[] { "PersonId" });
            DropIndex("dbo.GiftActivity", new[] { "Id" });
            DropIndex("dbo.Game", new[] { "ScoreId" });
            DropIndex("dbo.Game", new[] { "PublisherId" });
            DropIndex("dbo.Game", new[] { "DirectorId" });
            DropIndex("dbo.Game", new[] { "SeriesId" });
            DropIndex("dbo.Game", new[] { "PlatformId" });
            DropIndex("dbo.GameActivity", new[] { "GameId" });
            DropIndex("dbo.GameActivity", new[] { "Id" });
            DropIndex("dbo.EventActivity", new[] { "Id" });
            DropIndex("dbo.DoomActivity", new[] { "Id" });
            DropIndex("dbo.MediaType", new[] { "CategoryId" });
            DropIndex("dbo.ComicVolume", new[] { "ScoreId" });
            DropIndex("dbo.ComicVolume", new[] { "TypeId" });
            DropIndex("dbo.ComicVolume", new[] { "AuthorId" });
            DropIndex("dbo.ComicVolume", new[] { "SeriesId" });
            DropIndex("dbo.ComicActivity", new[] { "ComicVolumeId" });
            DropIndex("dbo.ComicActivity", new[] { "Id" });
            DropIndex("dbo.Franchise", new[] { "CreatorId" });
            DropIndex("dbo.Series", new[] { "FranchiseId" });
            DropIndex("dbo.Book", new[] { "ScoreId" });
            DropIndex("dbo.Book", new[] { "SeriesId" });
            DropIndex("dbo.Book", new[] { "AuthorId" });
            DropIndex("dbo.BookActivity", new[] { "BookId" });
            DropIndex("dbo.BookActivity", new[] { "Id" });
            DropIndex("dbo.AutoActivity", new[] { "AutoId" });
            DropIndex("dbo.AutoActivity", new[] { "Id" });
            DropIndex("dbo.Activity", new[] { "ActivityBlockId" });
            DropIndex("dbo.Activity", new[] { "PairedActivityId" });
            DropIndex("dbo.Activity", new[] { "StatusId" });
            DropIndex("dbo.Activity", new[] { "CategoryId" });
            DropTable("dbo.WorkActivity");
            DropTable("dbo.Location");
            DropTable("dbo.TravelActivity");
            DropTable("dbo.ActivityStatus");
            DropTable("dbo.ShowSeason");
            DropTable("dbo.ShowActivity");
            DropTable("dbo.PurchaseActivity");
            DropTable("dbo.PlayActivity");
            DropTable("dbo.PetActivity");
            DropTable("dbo.Movie");
            DropTable("dbo.MovieActivity");
            DropTable("dbo.MeetActivity");
            DropTable("dbo.HealthActivity");
            DropTable("dbo.GiftActivity");
            DropTable("dbo.Company");
            DropTable("dbo.Game");
            DropTable("dbo.GameActivity");
            DropTable("dbo.EventActivity");
            DropTable("dbo.DoomActivity");
            DropTable("dbo.MediaType");
            DropTable("dbo.ComicVolume");
            DropTable("dbo.ComicActivity");
            DropTable("dbo.ActivityCategory");
            DropTable("dbo.Franchise");
            DropTable("dbo.Series");
            DropTable("dbo.Score");
            DropTable("dbo.Person");
            DropTable("dbo.Book");
            DropTable("dbo.BookActivity");
            DropTable("dbo.Transport");
            DropTable("dbo.AutoActivity");
            DropTable("dbo.ActivityBlock");
            DropTable("dbo.Activity");
        }
    }
}

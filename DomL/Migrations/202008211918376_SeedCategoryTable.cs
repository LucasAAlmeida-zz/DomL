namespace DomL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class SeedCategoryTable : DbMigration
    {
        public override void Up()
        {
            Sql("" +
                "INSERT INTO ActivityCategory (Id, Name) VALUES (1, 'AUTO'); " +
                "INSERT INTO ActivityCategory (Id, Name) VALUES (2, 'BOOK'); " +
                "INSERT INTO ActivityCategory (Id, Name) VALUES (3, 'COMIC'); " +
                "INSERT INTO ActivityCategory (Id, Name) VALUES (4, 'DOOM'); " +
                "INSERT INTO ActivityCategory (Id, Name) VALUES (5, 'GIFT'); " +
                "INSERT INTO ActivityCategory (Id, Name) VALUES (6, 'HEALTH'); " +
                "INSERT INTO ActivityCategory (Id, Name) VALUES (7, 'MOVIE'); " +
                "INSERT INTO ActivityCategory (Id, Name) VALUES (8, 'PERSON'); " +
                "INSERT INTO ActivityCategory (Id, Name) VALUES (9, 'PET'); " +
                "INSERT INTO ActivityCategory (Id, Name) VALUES (10, 'PLAY'); " +
                "INSERT INTO ActivityCategory (Id, Name) VALUES (11, 'PURCHASE'); " +
                "INSERT INTO ActivityCategory (Id, Name) VALUES (12, 'TRAVEL'); " +
                "INSERT INTO ActivityCategory (Id, Name) VALUES (13, 'WORK'); " +
                "INSERT INTO ActivityCategory (Id, Name) VALUES (14, 'GAME'); " +
                "INSERT INTO ActivityCategory (Id, Name) VALUES (15, 'SERIES'); " +
                "INSERT INTO ActivityCategory (Id, Name) VALUES (16, 'WATCH'); " +
                "INSERT INTO ActivityCategory (Id, Name) VALUES (17, 'EVENT'); "
            );
        }
        
        public override void Down()
        {
            Sql("" +
                "DELETE FROM ActivityCategory WHERE ID = 1; " +
                "DELETE FROM ActivityCategory WHERE ID = 2; " +
                "DELETE FROM ActivityCategory WHERE ID = 3; " +
                "DELETE FROM ActivityCategory WHERE ID = 4; " +
                "DELETE FROM ActivityCategory WHERE ID = 5; " +
                "DELETE FROM ActivityCategory WHERE ID = 6; " +
                "DELETE FROM ActivityCategory WHERE ID = 7; " +
                "DELETE FROM ActivityCategory WHERE ID = 8; " +
                "DELETE FROM ActivityCategory WHERE ID = 9; " +
                "DELETE FROM ActivityCategory WHERE ID = 10; " +
                "DELETE FROM ActivityCategory WHERE ID = 11; " +
                "DELETE FROM ActivityCategory WHERE ID = 12; " +
                "DELETE FROM ActivityCategory WHERE ID = 13; " +
                "DELETE FROM ActivityCategory WHERE ID = 14; " +
                "DELETE FROM ActivityCategory WHERE ID = 15; " +
                "DELETE FROM ActivityCategory WHERE ID = 16; " +
                "DELETE FROM ActivityCategory WHERE ID = 17; "
            );
        }
    }
}

namespace DomL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class SeedStatusTable : DbMigration
    {
        public override void Up()
        {
            Sql("" +
               "INSERT INTO ActivityStatus (Id, Name) VALUES (1, 'SDA'); " +
               "INSERT INTO ActivityStatus (Id, Name) VALUES (2, 'Start'); " +
               "INSERT INTO ActivityStatus (Id, Name) VALUES (3, 'Finish'); "
            );
        }
        
        public override void Down()
        {
            Sql("" +
               "DELETE FROM ActivityStatus WHERE Id = 1; " +
               "DELETE FROM ActivityStatus WHERE Id = 2; " +
               "DELETE FROM ActivityStatus WHERE Id = 3; "
            );
        }
    }
}

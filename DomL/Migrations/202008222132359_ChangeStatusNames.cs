namespace DomL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ChangeStatusNames : DbMigration
    {
        public override void Up()
        {
            Sql(
                "UPDATE ActivityStatus SET Name='SINGLE' WHERE Name='SDA'; " +
                "UPDATE ActivityStatus SET Name='START' WHERE Name='Start'; " +
                "UPDATE ActivityStatus SET Name='FINISH' WHERE Name='Finish'; "
            );
        }

        public override void Down()
        {
            Sql(
                "UPDATE ActivityStatus SET Name='SDA' WHERE Name='SINGLE'; " +
                "UPDATE ActivityStatus SET Name='Start' WHERE Name='START'; " +
                "UPDATE ActivityStatus SET Name='Finish' WHERE Name='FINISH'; "
            );
        }
    }
}

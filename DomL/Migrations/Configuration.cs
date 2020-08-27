namespace DomL.Migrations
{
    using DomL.Business.Entities;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<DomL.DataAccess.DomLContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DomL.DataAccess.DomLContext context)
        {
            context.ActivityCategory.AddOrUpdate(u => u.Id,
                new ActivityCategory() { Id =  1, Name = "AUTO" },
                new ActivityCategory() { Id =  2, Name = "BOOK" },
                new ActivityCategory() { Id =  3, Name = "COMIC" },
                new ActivityCategory() { Id =  4, Name = "DOOM" },
                new ActivityCategory() { Id =  5, Name = "GIFT" },
                new ActivityCategory() { Id =  6, Name = "HEALTH" },
                new ActivityCategory() { Id =  7, Name = "MOVIE" },
                new ActivityCategory() { Id =  8, Name = "MEET" },
                new ActivityCategory() { Id =  9, Name = "PET" },
                new ActivityCategory() { Id = 10, Name = "PLAY" },
                new ActivityCategory() { Id = 11, Name = "PURCHASE" },
                new ActivityCategory() { Id = 12, Name = "TRAVEL" },
                new ActivityCategory() { Id = 13, Name = "WORK" },
                new ActivityCategory() { Id = 14, Name = "GAME" },
                new ActivityCategory() { Id = 15, Name = "SHOW" },
                new ActivityCategory() { Id = 17, Name = "EVENT" }
            );

            context.ActivityStatus.AddOrUpdate(u => u.Id,
                new ActivityStatus() { Id = 1, Name = "SINGLE" },
                new ActivityStatus() { Id = 2, Name = "START" },
                new ActivityStatus() { Id = 3, Name = "FINISH" }
            );
        }
    }
}

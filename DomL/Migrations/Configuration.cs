namespace DomL.Migrations
{
    using global::DomL.Business.Entities;
    using global::DomL.DataAccess;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<DomLContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DomLContext context)
        {
            context.ActivityCategory.AddOrUpdate(u => u.Id,
                new Category() { Id =  1, Name = "AUTO" },
                new Category() { Id =  2, Name = "BOOK" },
                new Category() { Id =  3, Name = "COMIC" },
                new Category() { Id =  4, Name = "DOOM" },
                new Category() { Id =  5, Name = "GIFT" },
                new Category() { Id =  6, Name = "HEALTH" },
                new Category() { Id =  7, Name = "MOVIE" },
                new Category() { Id =  8, Name = "MEET" },
                new Category() { Id =  9, Name = "PET" },
                new Category() { Id = 10, Name = "PLAY" },
                new Category() { Id = 11, Name = "PURCHASE" },
                new Category() { Id = 12, Name = "TRAVEL" },
                new Category() { Id = 13, Name = "WORK" },
                new Category() { Id = 14, Name = "GAME" },
                new Category() { Id = 15, Name = "SHOW" },
                new Category() { Id = 17, Name = "EVENT" },
                new Category() { Id = 18, Name = "COURSE" }
            );

            context.ActivityStatus.AddOrUpdate(u => u.Id,
                new Status() { Id = 1, Name = "SINGLE" },
                new Status() { Id = 2, Name = "START" },
                new Status() { Id = 3, Name = "FINISH" }
            );
        }
    }
}

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

            context.Score.AddOrUpdate(u => u.Id,
                new Score() { Id = 1, Name = "Worst thing", Value = 10 },
                new Score() { Id = 2, Name = "Horrible", Value = 20 },
                new Score() { Id = 3, Name = "Really bad", Value = 30 },
                new Score() { Id = 4, Name = "Bad", Value = 40 },
                new Score() { Id = 5, Name = "Meh", Value = 50 },
                new Score() { Id = 6, Name = "Ehh", Value = 55 },
                new Score() { Id = 7, Name = "Ok", Value = 60 },
                new Score() { Id = 8, Name = "Fine", Value = 65 },
                new Score() { Id = 9, Name = "Good", Value = 70 },
                new Score() { Id = 10, Name = "Nice", Value = 75 },
                new Score() { Id = 11, Name = "Really Good", Value = 80 },
                new Score() { Id = 12, Name = "Wow", Value = 85 },
                new Score() { Id = 13, Name = "Awesome", Value = 90 },
                new Score() { Id = 14, Name = "OMG", Value = 95 },
                new Score() { Id = 15, Name = "Best thing", Value = 100 }
            );

            context.MediaType.AddOrUpdate(u => u.Id,
                new MediaType() { Id = 1, Name = "Manga", CategoryId = 3 },
                new MediaType() { Id = 2, Name = "Comic", CategoryId = 3 },
                new MediaType() { Id = 3, Name = "WebComic", CategoryId = 3 },
                new MediaType() { Id = 4, Name = "SMS", CategoryId = 14 },
                new MediaType() { Id = 5, Name = "SMD", CategoryId = 14 },
                new MediaType() { Id = 6, Name = "NES", CategoryId = 14 },
                new MediaType() { Id = 7, Name = "SNES", CategoryId = 14 },
                new MediaType() { Id = 8, Name = "N64", CategoryId = 14 },
                new MediaType() { Id = 9, Name = "NGC", CategoryId = 14 },
                new MediaType() { Id = 10, Name = "Wii", CategoryId = 14 },
                new MediaType() { Id = 11, Name = "WiiU", CategoryId = 14 },
                new MediaType() { Id = 12, Name = "Switch", CategoryId = 14 },
                new MediaType() { Id = 13, Name = "NGB", CategoryId = 14 },
                new MediaType() { Id = 14, Name = "GBC", CategoryId = 14 },
                new MediaType() { Id = 15, Name = "GBA", CategoryId = 14 },
                new MediaType() { Id = 16, Name = "NDS", CategoryId = 14 },
                new MediaType() { Id = 17, Name = "3DS", CategoryId = 14 },
                new MediaType() { Id = 18, Name = "Arcade", CategoryId = 14 },
                new MediaType() { Id = 19, Name = "PC", CategoryId = 14 },
                new MediaType() { Id = 20, Name = "Watch", CategoryId = 14 },
                new MediaType() { Id = 21, Name = "PSX", CategoryId = 14 },
                new MediaType() { Id = 22, Name = "PS2", CategoryId = 14 },
                new MediaType() { Id = 23, Name = "PS3", CategoryId = 14 },
                new MediaType() { Id = 24, Name = "PS4", CategoryId = 14 },
                new MediaType() { Id = 25, Name = "PS5", CategoryId = 14 },
                new MediaType() { Id = 26, Name = "MXB", CategoryId = 14 },
                new MediaType() { Id = 27, Name = "XB360", CategoryId = 14 },
                new MediaType() { Id = 28, Name = "XB1", CategoryId = 14 },
                new MediaType() { Id = 29, Name = "XBX", CategoryId = 14 },
                new MediaType() { Id = 30, Name = "Anime", CategoryId = 15 },
                new MediaType() { Id = 31, Name = "Cartoon", CategoryId = 15 },
                new MediaType() { Id = 32, Name = "3D Animation", CategoryId = 15 },
                new MediaType() { Id = 33, Name = "TV Series", CategoryId = 15 }
            );
        }
    }
}

using DomL.Business.Entities;
using System.Data.Entity;

namespace DomL.DataAccess
{
    public class DomLContext : DbContext
    {
        public DbSet<Activity> Activity { get; set; }
        public DbSet<ActivityBlock> ActivityBlock { get; set; }
        public DbSet<ActivityCategory> ActivityCategory { get; set; }
        public DbSet<ActivityStatus> ActivityStatus { get; set; }

        public DbSet<AutoActivity> AutoActivity { get; set; }
        public DbSet<Auto> Auto { get; set; }

        public DbSet<BookActivity> BookActivity { get; set; }
        public DbSet<Book> Book { get; set; }

        public DbSet<ComicActivity> ComicActivity { get; set; }
        public DbSet<ComicVolume> ComicVolume { get; set; }

        public DbSet<DoomActivity> DoomActivity { get; set; }

        public DbSet<EventActivity> EventActivity { get; set; }

        public DbSet<GameActivity> GameActivity { get; set; }
        public DbSet<Game> Game { get; set; }

        public DbSet<GiftActivity> GiftActivity { get; set; }

        public DbSet<Person> Person { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<MediaType> MediaType { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Franchise> Franchise { get; set; }

        public DomLContext() : base("name=DefaultConnection") { }
    }
}

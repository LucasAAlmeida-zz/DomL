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

        public DbSet<BookActivity> BookActivity { get; set; }
        public DbSet<Book> Book { get; set; }

        public DbSet<ComicActivity> ComicActivity { get; set; }
        public DbSet<ComicVolume> ComicVolume { get; set; }

        public DbSet<DoomActivity> DoomActivity { get; set; }

        public DbSet<EventActivity> EventActivity { get; set; }

        public DbSet<GameActivity> GameActivity { get; set; }
        public DbSet<Game> Game { get; set; }

        public DbSet<GiftActivity> GiftActivity { get; set; }

        public DbSet<HealthActivity> HealthActivity { get; set; }

        public DbSet<MovieActivity> MovieActivity { get; set; }
        public DbSet<Movie> Movie { get; set; }

        public DbSet<PetActivity> PetActivity { get; set; }

        public DbSet<MeetActivity> MeetActivity { get; set; }

        public DbSet<PlayActivity> PlayActivity { get; set; }

        public DbSet<PurchaseActivity> PurchaseActivity { get; set; }

        public DbSet<ShowActivity> ShowActivity { get; set; }
        public DbSet<ShowSeason> ShowSeason { get; set; }

        public DbSet<TravelActivity> TravelActivity { get; set; }
        
        public DbSet<WorkActivity> WorkActivity { get; set; }

        public DbSet<Person> Person { get; set; }
        public DbSet<MediaType> MediaType { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<Transport> Transport { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<Franchise> Franchise { get; set; }
        public DbSet<Score> Score { get; set; }

        public DomLContext() : base("name=DefaultConnection") { }
    }
}

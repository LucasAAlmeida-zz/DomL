using DomL.Business.Entities;
using System.Data.Entity;

namespace DomL.DataAccess
{
    public class DomLContext : DbContext
    {
        public DbSet<Activity> Activity { get; set; }
        public DbSet<Category> ActivityCategory { get; set; }
        public DbSet<Status> ActivityStatus { get; set; }

        public DbSet<AutoActivity> AutoActivity { get; set; }

        public DbSet<BookActivity> BookActivity { get; set; }
        public DbSet<Book> Book { get; set; }

        public DbSet<ComicActivity> ComicActivity { get; set; }
        public DbSet<Comic> Comic { get; set; }

        public DbSet<CourseActivity> CourseActivity { get; set; }
        public DbSet<Course> Course { get; set; }

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
        public DbSet<Show> Show { get; set; }

        public DbSet<TravelActivity> TravelActivity { get; set; }
        
        public DbSet<WorkActivity> WorkActivity { get; set; }

        public DomLContext() : base("name=DefaultConnection") { }
    }
}

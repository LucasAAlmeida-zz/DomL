using DomL.Business;
using DomL.Business.Activities.MultipleDayActivities;
using DomL.Business.Activities.SingleDayActivities;
using System.Data.Entity;

namespace DomL.DataAccess
{
    public class DomLContext : DbContext
    {
        public DbSet<Book> Book { get; set; }
        public DbSet<Comic> Comic { get; set; }
        public DbSet<Game> Game { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<Watch> Watch { get; set; }
        public DbSet<Auto> Auto { get; set; }
        public DbSet<Doom> Doom { get; set; }
        public DbSet<Gift> Gift { get; set; }
        public DbSet<Health> Health { get; set; }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<Pet> Pet { get; set; }
        public DbSet<Play> Play { get; set; }
        public DbSet<Purchase> Purchase { get; set; }
        public DbSet<Travel> Travel { get; set; }
        public DbSet<Work> Work { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<ActivityBlock> ActivityBlock { get; set; }

        public DomLContext() : base("name=DefaultConnection") { }
    }
}

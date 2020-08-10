using DomL.Business.Activities.MultipleDayActivities;
using DomL.Business.Activities.SingleDayActivities;
using System.Data.Entity;

namespace DomL.DataAccess
{
    public class DomLContext : DbContext
    {
        DbSet<Book> Book { get; set; }
        DbSet<Comic> Comic { get; set; }
        DbSet<Game> Game { get; set; }
        DbSet<Series> Series { get; set; }
        DbSet<Watch> Watch { get; set; }
        DbSet<Auto> Auto { get; set; }
        DbSet<Doom> Doom { get; set; }
        DbSet<Gift> Gift { get; set; }
        DbSet<Health> Health { get; set; }
        DbSet<Movie> Movie { get; set; }
        DbSet<Person> Person { get; set; }
        DbSet<Pet> Pet { get; set; }
        DbSet<Play> Play { get; set; }
        DbSet<Purchase> Purchase { get; set; }
        DbSet<Travel> Travel { get; set; }
        DbSet<Work> Work { get; set; }

        public DomLContext()
            : base("name=DefaultConnection")
        {

        }
    }
}

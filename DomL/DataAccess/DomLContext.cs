using DomL.Business.Entities;
using System.Data.Entity;

namespace DomL.DataAccess
{
    public class DomLContext : DbContext
    {
        public DbSet<Activity> Activity { get; set; }
        public DbSet<ActivityBlock> ActivityBlock { get; set; }

        public DbSet<AutoActivity> AutoActivity { get; set; }
        public DbSet<Auto> Auto { get; set; }

        public DbSet<BookActivity> BookActivity { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<BookAuthor> BookAuthor { get; set; }
        public DbSet<BookSeries> BookSeries { get; set; }

        public DbSet<ComicActivity> ComicActivity { get; set; }
        public DbSet<ComicVolume> ComicVolume { get; set; }
        public DbSet<ComicAuthor> ComicAuthor { get; set; }
        public DbSet<ComicSeries> ComicSeries { get; set; }
        public DbSet<ComicType> ComicType { get; set; }


        public DomLContext() : base("name=DefaultConnection") { }
    }
}

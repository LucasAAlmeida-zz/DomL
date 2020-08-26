using DomL.Business.Entities;
using DomL.Business.Utils;
using System.Linq;
using System.Data.Entity;

namespace DomL.DataAccess
{
    public class MovieRepository : DomLRepository<MovieActivity>
    {
        public MovieRepository(DomLContext context) : base(context) { }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }

        public Movie GetMovieByTitle(string title)
        {
            var cleanTitle = Util.CleanString(title);
            return DomLContext.Movie
                .Include(u => u.Director)
                .Include(u => u.Series)
                .SingleOrDefault(u =>
                    u.Title.Replace(":", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").Replace("'", "").Replace(",", "").ToLower().Replace("the", "")
                    == cleanTitle
                );
        }

        public void CreateMovieActivity(MovieActivity movieActivity)
        {
            DomLContext.MovieActivity.Add(movieActivity);
        }

        public void CreateMovie(Movie movie)
        {
            DomLContext.Movie.Add(movie);
        }
    }
}

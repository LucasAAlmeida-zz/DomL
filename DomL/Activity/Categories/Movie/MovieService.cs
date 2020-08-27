using DomL.Business.Entities;
using DomL.Presentation;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;

namespace DomL.Business.Services
{
    public class MovieService
    {
        public static void SaveFromRawSegments(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            // MOVIE (Classification); Title; (Director Name); (Series Name); (Number In Series); (Score); (Description)
            segments[0] = "";
            var movieWindow = new MovieWindow(segments, activity, unitOfWork);

            if (ConfigurationManager.AppSettings["ShowCategoryWindows"] == "true") {
                movieWindow.ShowDialog();
            }

            var movieTitle = movieWindow.TitleCB.Text;
            var directorName = movieWindow.DirectorCB.Text;
            var seriesName = movieWindow.SeriesCB.Text;
            var numberInSeries = (!string.IsNullOrWhiteSpace(movieWindow.NumberCB.Text)) ? movieWindow.NumberCB.Text : null;
            var score = (!string.IsNullOrWhiteSpace(movieWindow.ScoreCB.Text)) ? movieWindow.ScoreCB.Text : null;
            var description = (!string.IsNullOrWhiteSpace(movieWindow.DescriptionCB.Text)) ? movieWindow.DescriptionCB.Text : null;

            Person director = PersonService.GetOrCreateByName(directorName, unitOfWork);
            Series series = SeriesService.GetOrCreateByName(seriesName, unitOfWork);

            Movie movie = GetOrUpdateOrCreateMovie(movieTitle, director, series, numberInSeries, score, unitOfWork);
            CreateMovieActivity(activity, movie, description, unitOfWork);
        }

        private static void CreateMovieActivity(Activity activity, Movie movie, string description, UnitOfWork unitOfWork)
        {
            var movieActivity = new MovieActivity() {
                Activity = activity,
                Movie = movie,
                Description = description
            };

            activity.MovieActivity = movieActivity;
            ActivityService.PairUpWithStartingActivity(activity, unitOfWork);

            unitOfWork.MovieRepo.CreateMovieActivity(movieActivity);
        }

        private static Movie GetOrUpdateOrCreateMovie(string movieTitle, Person director, Series series, string numberInSeries, string score, UnitOfWork unitOfWork)
        {
            var movie = unitOfWork.MovieRepo.GetMovieByTitle(movieTitle);

            if (movie == null) {
                movie = new Movie() {
                    Director = director,
                    Series = series,
                    Title = movieTitle,
                    NumberInSeries = numberInSeries,
                    Score = score,
                };
                unitOfWork.MovieRepo.CreateMovie(movie);
            } else {
                movie.Director = director ?? movie.Director;
                movie.Series = series ?? movie.Series;
            }

            return movie;
        }

        public static Movie GetByTitle(string title, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(title)) {
                return null;
            }
            return unitOfWork.MovieRepo.GetMovieByTitle(title);
        }

        public static IEnumerable<Activity> GetStartingActivities(IQueryable<Activity> previousStartingActivities, Activity activity)
        {
            var movie = activity.MovieActivity.Movie;
            return previousStartingActivities.Where(u =>
                u.CategoryId == ActivityCategory.MOVIE_ID
                && u.MovieActivity.Movie.Title == movie.Title
            );
        }
    }
}

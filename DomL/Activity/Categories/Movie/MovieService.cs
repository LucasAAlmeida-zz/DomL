using DomL.Business.Entities;
using DomL.Presentation;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.IO;
using System;
using DomL.DataAccess;
using System.Text.RegularExpressions;
using DomL.Business.DTOs;

namespace DomL.Business.Services
{
    public class MovieService
    {
        public static void SaveFromRawSegments(string[] rawSegments, Activity activity, UnitOfWork unitOfWork)
        {
            // MOVIE (Classification); Title; (Director Name); (Series Name); (Number In Series); (Score); (Description)
            rawSegments[0] = "";
            var movieWindow = new MovieWindow(rawSegments, activity, unitOfWork);

            if (ConfigurationManager.AppSettings["ShowCategoryWindows"] == "true") {
                movieWindow.ShowDialog();
            }

            var consolidated = new MovieConsolidatedDTO(movieWindow, activity);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        public static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            var consolidated = new MovieConsolidatedDTO(backupSegments);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        private static void SaveFromConsolidated(MovieConsolidatedDTO consolidated, UnitOfWork unitOfWork)
        {
            var series = SeriesService.GetOrCreateByName(consolidated.SeriesName, unitOfWork);
            var movie = GetOrUpdateOrCreateMovie(consolidated, series, unitOfWork);
            var activity = ActivityService.Create(consolidated, unitOfWork);

            CreateMovieActivity(activity, movie, consolidated.Description, unitOfWork);
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

        private static Movie GetOrUpdateOrCreateMovie(MovieConsolidatedDTO consolidated, Series series, UnitOfWork unitOfWork)
        {
            var movie = unitOfWork.MovieRepo.GetMovieByTitle(consolidated.Title);

            if (movie == null) {
                movie = new Movie() {
                    Person = consolidated.Person,
                    Series = series,
                    Title = consolidated.Title,
                    Number = consolidated.Number,
                    Company = consolidated.Company,
                    Year = int.Parse(consolidated.Year),
                    Score = consolidated.Score,
                };
                unitOfWork.MovieRepo.CreateMovie(movie);
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

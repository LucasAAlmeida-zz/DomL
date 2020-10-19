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

            var consolidated = new ConsolidatedMovieDTO(movieWindow, activity);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        public static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            var consolidated = new ConsolidatedMovieDTO(backupSegments);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        private static void SaveFromConsolidated(ConsolidatedMovieDTO consolidated, UnitOfWork unitOfWork)
        {
            var director = PersonService.GetOrCreateByName(consolidated.DirectorName, unitOfWork);
            var series = SeriesService.GetOrCreateByName(consolidated.SeriesName, unitOfWork);
            var score = ScoreService.GetByValue(consolidated.ScoreValue, unitOfWork);

            var movie = GetOrUpdateOrCreateMovie(consolidated.Title, director, series, consolidated.NumberInSeries, score, unitOfWork);

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

        private static Movie GetOrUpdateOrCreateMovie(string movieTitle, Person director, Series series, string numberInSeries, Score score, UnitOfWork unitOfWork)
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
                movie.Score = score ?? movie.Score;
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

        public static void RestoreFromFile(string fileDir)
        {
            using (var reader = new StreamReader(fileDir + "Movie.txt")) {
                string line = "";
                while ((line = reader.ReadLine()) != null) {
                    if (string.IsNullOrWhiteSpace(line)) {
                        continue;
                    }

                    var segments = Regex.Split(line, "\t");

                    // Date; Title; (Director Name); (Series Name); (Number In Series); (Score); (Description)
                    var date = segments[0];
                    var title = segments[1];
                    var seriesName = segments[2] != "-" ? segments[2] : null;
                    var numberInSeries = segments[3] != "-" ? segments[3] : null;
                    var directorName = segments[4] != "-" ? segments[4] : null;
                    var scoreValue = segments[5] != "-" ? segments[5] : null;
                    var description = segments[6] != "-" ? segments[6] : null;

                    var originalLine = "MOVIE; " + title;
                    originalLine = (!string.IsNullOrWhiteSpace(seriesName)) ? originalLine + "; " + seriesName : originalLine;
                    originalLine = (!string.IsNullOrWhiteSpace(numberInSeries)) ? originalLine + "; " + numberInSeries : originalLine;
                    originalLine = (!string.IsNullOrWhiteSpace(directorName)) ? originalLine + "; " + directorName : originalLine;
                    originalLine = (!string.IsNullOrWhiteSpace(scoreValue)) ? originalLine + "; " + scoreValue : originalLine;
                    originalLine = (!string.IsNullOrWhiteSpace(description)) ? originalLine + "; " + description : originalLine;

                    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                        var series = SeriesService.GetOrCreateByName(seriesName, unitOfWork);
                        var director = PersonService.GetOrCreateByName(directorName, unitOfWork);
                        var score = ScoreService.GetByValue(scoreValue, unitOfWork);
                        var movie = GetOrUpdateOrCreateMovie(title, director, series, numberInSeries, score, unitOfWork);

                        var statusSingle = unitOfWork.ActivityRepo.GetStatusById(ActivityStatus.SINGLE);
                        var category = unitOfWork.ActivityRepo.GetCategoryById(ActivityCategory.MOVIE_ID);
                        var dateDT = DateTime.ParseExact(date, "dd/MM/yy", null);
                        var activity = ActivityService.Create(dateDT, 0, statusSingle, category, null, originalLine, unitOfWork);

                        CreateMovieActivity(activity, movie, description, unitOfWork);

                        unitOfWork.Complete();
                    }
                }
            }
        }
    }
}

using DomL.Business.Entities;
using DomL.DataAccess;
using DomL.Presentation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DomL.Business.Services
{
    public class GameService
    {
        public static void SaveFromRawSegments(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            // GAME (Classification); Title; Platform Name; (Series Name); (Number In Series); (Director Name); (Publisher Name); (Score); (Description)
            segments[0] = "";
            var gameWindow = new GameWindow(segments, activity, unitOfWork);

            if (ConfigurationManager.AppSettings["ShowCategoryWindows"] == "true") {
                gameWindow.ShowDialog();
            }

            var title = gameWindow.TitleCB.Text;
            var platformName = gameWindow.PlatformCB.Text;
            var seriesName = gameWindow.SeriesCB.Text;
            var numberInSeries = (!string.IsNullOrWhiteSpace(gameWindow.NumberCB.Text)) ? gameWindow.NumberCB.Text : null;
            var directorName = gameWindow.DirectorCB.Text;
            var publisherName = gameWindow.PublisherCB.Text;
            var scoreValue = gameWindow.ScoreCB.Text;
            var description = (!string.IsNullOrWhiteSpace(gameWindow.DescriptionCB.Text)) ? gameWindow.DescriptionCB.Text : null;

            var platform = MediaTypeService.GetByName(platformName, unitOfWork);
            var series = SeriesService.GetOrCreateByName(seriesName, unitOfWork);
            var director = PersonService.GetOrCreateByName(directorName, unitOfWork);
            var publisher = CompanyService.GetOrCreateByName(publisherName, unitOfWork);
            var score = ScoreService.GetByValue(scoreValue, unitOfWork);

            Game game = GetOrUpdateOrCreateGame(title, platform, series, numberInSeries, director, publisher, score, unitOfWork);
            CreateGameActivity(activity, game, description, unitOfWork);
        }

        public static List<Game> GetAll(UnitOfWork unitOfWork)
        {
            return unitOfWork.GameRepo.GetAllGames();
        }

        private static Game GetOrUpdateOrCreateGame(string title, MediaType platform, Series series, string numberInSeries, Person director, Company publisher, Score score, UnitOfWork unitOfWork)
        {
            var game = GetGameByTitleAndPlatformName(title, platform.Name, unitOfWork);

            if (game == null) {
                game = new Game() {
                    Title = title,
                    Platform = platform,
                    Series = series,
                    NumberInSeries = numberInSeries,
                    Director = director,
                    Publisher = publisher,
                    Score = score,
                };
                unitOfWork.GameRepo.CreateGame(game);
            } else {
                game.Series = series ?? game.Series;
                game.Director = director ?? game.Director;
                game.Publisher = publisher ?? game.Publisher;
                game.Score = score ?? game.Score;
            }

            return game;
        }

        private static void CreateGameActivity(Activity activity, Game game, string description, UnitOfWork unitOfWork)
        {
            var gameActivity = new GameActivity() {
                Activity = activity,
                Game = game,
                Description = description
            };

            activity.GameActivity = gameActivity;
            ActivityService.PairUpWithStartingActivity(activity, unitOfWork);

            unitOfWork.GameRepo.CreateGameActivity(gameActivity);
        }

        public static IEnumerable<Activity> GetStartingActivities(IQueryable<Activity> previousStartingActivities, Activity activity)
        {
            var game = activity.GameActivity.Game;
            return previousStartingActivities.Where(u => 
                u.CategoryId == ActivityCategory.GAME_ID
                && u.GameActivity.Game.Title == game.Title && u.GameActivity.Game.Platform.Name == game.Platform.Name
            );
        }

        public static Game GetGameByTitle(string title, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(title)) {
                return null;
            }
            return unitOfWork.GameRepo.GetGameByTitle(title);
        }

        public static Game GetGameByTitleAndPlatformName(string title, string platformName, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(platformName)) {
                return null;
            }
            return unitOfWork.GameRepo.GetGameByTitleAndPlatformName(title, platformName);
        }

        public static void RestoreFromFile(string fileDir)
        {
            using (var reader = new StreamReader(fileDir + "Game.txt")) {
                string line = "";
                while ((line = reader.ReadLine()) != null) {
                    if (string.IsNullOrWhiteSpace(line)) {
                        continue;
                    }

                    var segments = Regex.Split(line, "\t");

                    // Date Start; Date Finish; Title; Platform Name; (Series Name); (Number In Series); (Director Name); (Publisher Name); (Score); (Description)
                    var dateStart = segments[0];
                    var dateFinish = segments[1];
                    var title = segments[2];
                    var platformName = segments[3];
                    var seriesName = segments[4] != "-" ? segments[4] : null;
                    var numberInSeries = segments[5] != "-" ? segments[5] : null;
                    var directorName = segments[6] != "-" ? segments[6] : null;
                    var publisherName = segments[7] != "-" ? segments[7] : null;
                    var scoreValue = segments[8] != "-" ? segments[8] : null;
                    var description = segments[9] != "-" ? segments[9] : null;

                    var originalLine = title + "; " + platformName;
                    originalLine = (!string.IsNullOrWhiteSpace(seriesName)) ? originalLine + "; " + seriesName : originalLine;
                    originalLine = (!string.IsNullOrWhiteSpace(numberInSeries)) ? originalLine + "; " + numberInSeries : originalLine;
                    originalLine = (!string.IsNullOrWhiteSpace(directorName)) ? originalLine + "; " + directorName : originalLine;
                    originalLine = (!string.IsNullOrWhiteSpace(publisherName)) ? originalLine + "; " + publisherName : originalLine;
                    originalLine = (!string.IsNullOrWhiteSpace(scoreValue)) ? originalLine + "; " + scoreValue : originalLine;
                    originalLine = (!string.IsNullOrWhiteSpace(description)) ? originalLine + "; " + description : originalLine;

                    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                        var platform = MediaTypeService.GetByName(platformName, unitOfWork);
                        var series = SeriesService.GetOrCreateByName(seriesName, unitOfWork);
                        var director = PersonService.GetOrCreateByName(directorName, unitOfWork);
                        var publisher = CompanyService.GetOrCreateByName(publisherName, unitOfWork);
                        var score = ScoreService.GetByValue(scoreValue, unitOfWork);

                        Game game = GetOrUpdateOrCreateGame(title, platform, series, numberInSeries, director, publisher, score, unitOfWork);

                        var statusStart = unitOfWork.ActivityRepo.GetStatusById(ActivityStatus.START);
                        var statusFinish = unitOfWork.ActivityRepo.GetStatusById(ActivityStatus.FINISH);
                        var statusSingle = unitOfWork.ActivityRepo.GetStatusById(ActivityStatus.SINGLE);

                        var category = unitOfWork.ActivityRepo.GetCategoryById(ActivityCategory.GAME_ID);

                        if (!dateStart.StartsWith("--") && dateStart != dateFinish) {
                            Activity activityStart = null;
                            Activity activityFinish = null;
                            if (!dateStart.StartsWith("??")) {
                                var date = DateTime.ParseExact(dateStart, "dd/MM/yy", null);
                                activityStart = ActivityService.Create(date, 0, statusStart, category, null, "GAME Start; " + originalLine, unitOfWork);
                                CreateGameActivity(activityStart, game, description, unitOfWork);
                            }
                            if (!dateFinish.StartsWith("??")) {
                                var date = DateTime.ParseExact(dateFinish, "dd/MM/yy", null);
                                activityFinish = ActivityService.Create(date, 0, statusFinish, category, null, "GAME Finish; " + originalLine, unitOfWork);
                                CreateGameActivity(activityFinish, game, description, unitOfWork);
                            }

                            if (activityStart != null && activityFinish != null) {
                                activityStart.GameActivity.Description = null;
                                activityStart.PairedActivity = activityFinish;
                                unitOfWork.Complete();
                                activityFinish.PairedActivity = activityStart;
                            }
                        } else {
                            var date = DateTime.ParseExact(dateFinish, "dd/MM/yy", null);
                            var activity = ActivityService.Create(date, 0, statusSingle, category, null, "GAME; " + originalLine, unitOfWork);
                            CreateGameActivity(activity, game, description, unitOfWork);
                        }

                        unitOfWork.Complete();
                    }
                }
            }
        }
    }
}

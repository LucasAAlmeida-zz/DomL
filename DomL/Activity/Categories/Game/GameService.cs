using DomL.Business.DTOs;
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
        public static void SaveFromRawSegments(string[] rawSegments, Activity activity, UnitOfWork unitOfWork)
        {
            // GAME (Classification); Title; Platform Name; (Series Name); (Number In Series); (Director Name); (Publisher Name); (Score); (Description)
            rawSegments[0] = "";
            var gameWindow = new GameWindow(rawSegments, activity, unitOfWork);

            if (ConfigurationManager.AppSettings["ShowCategoryWindows"] == "true") {
                gameWindow.ShowDialog();
            }

            var consolidated = new ConsolidatedGameDTO(gameWindow, activity);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        public static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            var consolidated = new ConsolidatedGameDTO(backupSegments);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        private static void SaveFromConsolidated(ConsolidatedGameDTO consolidated, UnitOfWork unitOfWork)
        {
            var platform = MediaTypeService.GetByName(consolidated.PlatformName, unitOfWork);
            var series = SeriesService.GetOrCreateByName(consolidated.SeriesName, unitOfWork);
            var director = PersonService.GetOrCreateByName(consolidated.DirectorName, unitOfWork);
            var publisher = CompanyService.GetOrCreateByName(consolidated.PublisherName, unitOfWork);
            var score = ScoreService.GetByValue(consolidated.ScoreValue, unitOfWork);

            var game = GetOrUpdateOrCreateGame(consolidated.Title, platform, series, consolidated.NumberInSeries, director, publisher, score, unitOfWork);

            var activity = ActivityService.Create(consolidated, unitOfWork);
            CreateGameActivity(activity, game, consolidated.Description, unitOfWork);
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
    }
}

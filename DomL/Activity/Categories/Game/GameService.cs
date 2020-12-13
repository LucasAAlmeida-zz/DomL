using DomL.Business.DTOs;
using DomL.Business.Entities;
using DomL.Business.Utils;
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
            var series = SeriesService.GetOrCreateByName(consolidated.SeriesName, unitOfWork);
            var game = GetOrUpdateOrCreateGame(consolidated, series, unitOfWork);
            var activity = ActivityService.Create(consolidated, unitOfWork);
            CreateGameActivity(activity, game, consolidated.Description, unitOfWork);
        }

        public static List<Game> GetAll(UnitOfWork unitOfWork)
        {
            return unitOfWork.GameRepo.GetAllGames();
        }

        private static Game GetOrUpdateOrCreateGame(ConsolidatedGameDTO consolidated, Series series, UnitOfWork unitOfWork)
        {
            var game = GetGameByTitle(consolidated.Title, unitOfWork);

            if (game == null) {
                game = new Game() {
                    Title = Util.GetStringOrNull(consolidated.Title),
                    Platform = Util.GetStringOrNull(consolidated.Platform),
                    Series = series,
                    Number = Util.GetStringOrNull(consolidated.Number),
                    Person = Util.GetStringOrNull(consolidated.Person),
                    Company = Util.GetStringOrNull(consolidated.Company),
                    Year = Util.GetIntOrZero(consolidated.Year),
                    Score = Util.GetStringOrNull(consolidated.Score),
                };
                unitOfWork.GameRepo.CreateGame(game);
            } else {
                game.Series = series ?? game.Series;
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

        //TODO add year to search
        public static IEnumerable<Activity> GetStartingActivities(IQueryable<Activity> previousStartingActivities, Activity activity)
        {
            var game = activity.GameActivity.Game;
            return previousStartingActivities.Where(u => 
                u.CategoryId == ActivityCategory.GAME_ID
                && u.GameActivity.Game.Title == game.Title
            );
        }

        public static Game GetGameByTitle(string title, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(title)) {
                return null;
            }
            return unitOfWork.GameRepo.GetGameByTitle(title);
        }
    }
}

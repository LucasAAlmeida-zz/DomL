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
            var instance = GetByTitle(consolidated.Title, unitOfWork);

            var title = Util.GetStringOrNull(consolidated.Title);
            var platform = Util.GetStringOrNull(consolidated.Platform);
            var number = Util.GetStringOrNull(consolidated.Number);
            var person = Util.GetStringOrNull(consolidated.Person);
            var company = Util.GetStringOrNull(consolidated.Company);
            var year = Util.GetIntOrZero(consolidated.Year);
            var score = Util.GetStringOrNull(consolidated.Score);

            if (instance == null) {
                instance = new Game() {
                    Title = title,
                    Type = platform,
                    Series = series,
                    Number = number,
                    Person = person,
                    Company = company,
                    Year = year,
                    Score = score,
                };
            } else {
                instance.Type = platform ?? instance.Type;
                instance.Series = series ?? instance.Series;
                instance.Number = number ?? instance.Number;
                instance.Person = person ?? instance.Person;
                instance.Company = company ?? instance.Company;
                instance.Year = year != 0 ? year : instance.Year;
                instance.Score = score ?? instance.Score;
            }

            return instance;
        }

        private static void CreateGameActivity(Activity activity, Game game, string description, UnitOfWork unitOfWork)
        {
            var gameActivity = new GameActivity() {
                Activity = activity,
                Game = game,
                Description = Util.GetStringOrNull(description)
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
                u.CategoryId == Category.GAME_ID
                && u.GameActivity.Game.Title == game.Title
            );
        }

        public static Game GetByTitle(string title, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(title)) {
                return null;
            }
            return unitOfWork.GameRepo.GetGameByTitle(title);
        }
    }
}

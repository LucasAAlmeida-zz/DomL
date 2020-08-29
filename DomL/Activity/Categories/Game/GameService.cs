﻿using DomL.Business.Entities;
using DomL.Presentation;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

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

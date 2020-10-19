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
    public class ShowService
    {
        public static void SaveFromRawSegments(string[] rawSegments, Activity activity, UnitOfWork unitOfWork)
        {
            rawSegments[0] = "";
            var showWindow = new ShowWindow(rawSegments, activity, unitOfWork);

            if (ConfigurationManager.AppSettings["ShowCategoryWindows"] == "true") {
                showWindow.ShowDialog();
            }

            var consolidated = new ConsolidatedShowDTO(showWindow, activity);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        public static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            var consolidated = new ConsolidatedShowDTO(backupSegments);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        private static void SaveFromConsolidated(ConsolidatedShowDTO consolidated, UnitOfWork unitOfWork)
        {
            var series = SeriesService.GetOrCreateByName(consolidated.SeriesName, unitOfWork);
            var director = PersonService.GetOrCreateByName(consolidated.DirectorName, unitOfWork);
            var type = MediaTypeService.GetByName(consolidated.TypeName, unitOfWork);
            var score = ScoreService.GetByValue(consolidated.ScoreValue, unitOfWork);

            var showSeason = GetOrUpdateOrCreateShowSeason(series, consolidated.Season, director, type, score, unitOfWork);

            var activity = ActivityService.Create(consolidated, unitOfWork);
            CreateShowActivity(activity, showSeason, consolidated.Description, unitOfWork);
        }

        private static void CreateShowActivity(Activity activity, ShowSeason showSeason, string description, UnitOfWork unitOfWork)
        {
            var showActivity = new ShowActivity() {
                Activity = activity,
                ShowSeason = showSeason,
                Description = description
            };

            activity.ShowActivity = showActivity;
            ActivityService.PairUpWithStartingActivity(activity, unitOfWork);

            unitOfWork.ShowRepo.CreateShowActivity(showActivity);
        }

        private static ShowSeason GetOrUpdateOrCreateShowSeason(Series series, string season, Person director, MediaType type, Score score, UnitOfWork unitOfWork)
        {
            var showSeason = GetShowSeasonBySeriesNameAndSeason(series.Name, season, unitOfWork);

            if (showSeason == null) {
                showSeason = new ShowSeason() {
                    Series = series,
                    Season = season,
                    Director = director,
                    Type = type,
                    Score = score,
                };
                unitOfWork.ShowRepo.CreateShowSeason(showSeason);
            } else {
                showSeason.Director = director ?? showSeason.Director;
                showSeason.Type = type ?? showSeason.Type;
                showSeason.Score = score ?? showSeason.Score;
            }

            return showSeason;
        }

        public static ShowSeason GetShowSeasonBySeriesNameAndSeason(string seriesName, string season, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(seriesName) || string.IsNullOrWhiteSpace(season)) {
                return null;
            }
            return unitOfWork.ShowRepo.GetShowSeasonBySeriesNameAndSeason(seriesName, season);
        }

        public static IEnumerable<Activity> GetStartingActivities(IQueryable<Activity> previousStartingActivities, Activity activity)
        {
            var showSeason = activity.ShowActivity.ShowSeason;
            return previousStartingActivities.Where(u => 
                u.CategoryId == ActivityCategory.SHOW_ID
                && u.ShowActivity.ShowSeason.Series.Name == showSeason.Series.Name && u.ShowActivity.ShowSeason.Season == showSeason.Season
            );
        }
    }
}

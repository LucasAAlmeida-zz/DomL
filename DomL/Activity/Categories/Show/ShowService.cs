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
    public class ShowService
    {
        public static void SaveFromRawSegments(string[] rawSegments, Activity activity, UnitOfWork unitOfWork)
        {
            rawSegments[0] = "";
            var showWindow = new ShowWindow(rawSegments, activity, unitOfWork);

            if (ConfigurationManager.AppSettings["ShowCategoryWindows"] == "true") {
                showWindow.ShowDialog();
            }

            var consolidated = new ShowConsolidatedDTO(showWindow, activity);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        public static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            var consolidated = new ShowConsolidatedDTO(backupSegments);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        private static void SaveFromConsolidated(ShowConsolidatedDTO consolidated, UnitOfWork unitOfWork)
        {
            var series = SeriesService.GetOrCreateByName(consolidated.SeriesName, unitOfWork);
            var showSeason = GetOrUpdateOrCreateShowSeason(consolidated, series, unitOfWork);
            var activity = ActivityService.Create(consolidated, unitOfWork);

            CreateShowActivity(activity, showSeason, consolidated.Description, unitOfWork);
        }

        private static void CreateShowActivity(Activity activity, Show showSeason, string description, UnitOfWork unitOfWork)
        {
            var showActivity = new ShowActivity() {
                Activity = activity,
                Show = showSeason,
                Description = description
            };

            activity.ShowActivity = showActivity;
            ActivityService.PairUpWithStartingActivity(activity, unitOfWork);

            unitOfWork.ShowRepo.CreateShowActivity(showActivity);
        }

        private static Show GetOrUpdateOrCreateShowSeason(ShowConsolidatedDTO consolidated, Series series, UnitOfWork unitOfWork)
        {
            var showSeason = GetShowByTitle(consolidated.Title, unitOfWork);

            if (showSeason == null) {
                showSeason = new Show() {
                    Title = Util.GetStringOrNull(consolidated.Title),
                    Type = Util.GetStringOrNull(consolidated.Type),
                    Series = series,
                    Number = Util.GetStringOrNull(consolidated.Number),
                    Person = Util.GetStringOrNull(consolidated.Person),
                    Company = Util.GetStringOrNull(consolidated.Company),
                    Year = Util.GetIntOrZero(consolidated.Year),
                    Score = Util.GetStringOrNull(consolidated.Score),
                };
                unitOfWork.ShowRepo.CreateShowSeason(showSeason);
            }

            return showSeason;
        }

        //TODO colocar year na search
        public static Show GetShowByTitle(string title, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(title)) {
                return null;
            }
            return unitOfWork.ShowRepo.GetShowByTitle(title);
        }

        public static IEnumerable<Activity> GetStartingActivities(IQueryable<Activity> previousStartingActivities, Activity activity)
        {
            var showSeason = activity.ShowActivity.Show;
            return previousStartingActivities.Where(u => 
                u.CategoryId == ActivityCategory.SHOW_ID
                && u.ShowActivity.Show.Series.Name == showSeason.Series.Name && u.ShowActivity.Show.Title == showSeason.Title
            );
        }
    }
}

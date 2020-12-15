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

        public static List<Show> GetAll(UnitOfWork unitOfWork)
        {
            return unitOfWork.ShowRepo.GetAllShows();
        }

        private static void SaveFromConsolidated(ShowConsolidatedDTO consolidated, UnitOfWork unitOfWork)
        {
            var showSeason = GetOrUpdateOrCreateShowSeason(consolidated, unitOfWork);
            var activity = ActivityService.Create(consolidated, unitOfWork);
            CreateShowActivity(activity, showSeason, consolidated.Description, unitOfWork);
        }

        private static void CreateShowActivity(Activity activity, Show showSeason, string description, UnitOfWork unitOfWork)
        {
            var showActivity = new ShowActivity() {
                Activity = activity,
                Show = showSeason,
                Description = Util.GetStringOrNull(description)
            };

            activity.ShowActivity = showActivity;
            ActivityService.PairUpWithStartingActivity(activity, unitOfWork);

            unitOfWork.ShowRepo.CreateShowActivity(showActivity);
        }

        private static Show GetOrUpdateOrCreateShowSeason(ShowConsolidatedDTO consolidated, UnitOfWork unitOfWork)
        {
            var instance = GetByTitle(consolidated.Title, unitOfWork);

            var title = Util.GetStringOrNull(consolidated.Title);
            var type = Util.GetStringOrNull(consolidated.Type);
            var series = Util.GetStringOrNull(consolidated.Series);
            var number = Util.GetStringOrNull(consolidated.Number);
            var person = Util.GetStringOrNull(consolidated.Person);
            var company = Util.GetStringOrNull(consolidated.Company);
            var year = Util.GetStringOrNull(consolidated.Year);
            var score = Util.GetStringOrNull(consolidated.Score);

            if (instance == null) {
                instance = new Show() {
                    Title = title,
                    Type = type,
                    Series = series,
                    Number = number,
                    Person = person,
                    Company = company,
                    Year = year,
                    Score = score,
                };
            } else {
                instance.Type = type ?? instance.Type;
                instance.Series = series ?? instance.Series;
                instance.Number = number ?? instance.Number;
                instance.Person = person ?? instance.Person;
                instance.Company = company ?? instance.Company;
                instance.Year = year ?? instance.Year;
                instance.Score = score ?? instance.Score;
            }

            return instance;
        }

        //TODO colocar year na search
        public static Show GetByTitle(string title, UnitOfWork unitOfWork)
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
                u.CategoryId == Category.SHOW_ID
                && u.ShowActivity.Show.Series == showSeason.Series && u.ShowActivity.Show.Title == showSeason.Title
            );
        }
    }
}

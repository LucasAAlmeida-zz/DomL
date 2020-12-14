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
    public class ComicService
    {
        public static void SaveFromRawSegments(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            segments[0] = "";
            var comicWindow = new ComicWindow(segments, activity, unitOfWork);

            if (ConfigurationManager.AppSettings["ShowCategoryWindows"] == "true") {
                comicWindow.ShowDialog();
            }

            var consolidated = new ComicConsolidatedDTO(comicWindow, activity);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        public static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            var consolidated = new ComicConsolidatedDTO(backupSegments);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        private static void SaveFromConsolidated(ComicConsolidatedDTO consolidated, UnitOfWork unitOfWork)
        {
            var series = SeriesService.GetOrCreateByName(consolidated.Series, unitOfWork);

            var comicVolume = GetOrUpdateOrCreateComicVolume(consolidated, series, unitOfWork);
            var activity = ActivityService.Create(consolidated, unitOfWork);
            CreateComicActivity(activity, comicVolume, consolidated.Description, unitOfWork);
        }

        private static Comic GetOrUpdateOrCreateComicVolume(ComicConsolidatedDTO consolidated, Series series, UnitOfWork unitOfWork)
        {
            var instance = GetByTitle(consolidated.Title, unitOfWork);

            var title = Util.GetStringOrNull(consolidated.Title);
            var type = Util.GetStringOrNull(consolidated.Type);
            var number = Util.GetStringOrNull(consolidated.Number);
            var person = Util.GetStringOrNull(consolidated.Person);
            var company = Util.GetStringOrNull(consolidated.Company);
            var year = Util.GetIntOrZero(consolidated.Year);
            var score = Util.GetStringOrNull(consolidated.Score);

            if (instance == null) {
                instance = new Comic() {
                    Title = title,
                    Type = type,
                    Series = series,
                    Number = number,
                    Author = person,
                    Publisher = company,
                    Year = year,
                    Score = score,
                };
            } else {
                instance.Type = type ?? instance.Type;
                instance.Series = series ?? instance.Series;
                instance.Number = number ?? instance.Number;
                instance.Author = person ?? instance.Author;
                instance.Publisher = company ?? instance.Publisher;
                instance.Year = year != 0 ? year : instance.Year;
                instance.Score = score ?? instance.Score;
            }

            return instance;
        }

        private static void CreateComicActivity(Activity activity, Comic comic, string description, UnitOfWork unitOfWork)
        {
            var comicActivity = new ComicActivity() {
                Activity = activity,
                Comic = comic,
                Description = Util.GetStringOrNull(description)
            };

            activity.ComicActivity = comicActivity;
            ActivityService.PairUpWithStartingActivity(activity, unitOfWork);

            unitOfWork.ComicRepo.CreateComicActivity(comicActivity);
        }

        //TODO add year to search
        public static Comic GetByTitle(string title, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(title)) {
                return null;
            }
            return unitOfWork.ComicRepo.GetComicByTitle(title);
        }

        //TODO add year to search
        public static IEnumerable<Activity> GetStartingActivities(IQueryable<Activity> previousStartingActivities, Activity activity)
        {
            var comic = activity.ComicActivity.Comic;
            return previousStartingActivities.Where(u => 
                u.CategoryId == Category.COMIC_ID
                && u.ComicActivity.Comic.Title == comic.Title
            );
        }
    }
}

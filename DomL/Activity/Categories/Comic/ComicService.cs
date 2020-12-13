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
            var series = SeriesService.GetOrCreateByName(consolidated.SeriesName, unitOfWork);

            var comicVolume = GetOrUpdateOrCreateComicVolume(consolidated, series, unitOfWork);
            var activity = ActivityService.Create(consolidated, unitOfWork);
            CreateComicActivity(activity, comicVolume, consolidated.Description, unitOfWork);
        }

        private static Comic GetOrUpdateOrCreateComicVolume(ComicConsolidatedDTO consolidated, Series series, UnitOfWork unitOfWork)
        {
            var comic = GetComicByTitle(consolidated.Title, unitOfWork);

            if (comic == null) {
                comic = new Comic() {
                    Title = Util.GetStringOrNull(consolidated.Title),
                    Author = Util.GetStringOrNull(consolidated.Author),
                    Type = Util.GetStringOrNull(consolidated.Type),
                    Series = series,
                    Number = Util.GetStringOrNull(consolidated.Number),
                    Publisher = Util.GetStringOrNull(consolidated.Publisher),
                    Year = Util.GetIntOrZero(consolidated.Year),
                    Score = Util.GetStringOrNull(consolidated.Score),
                };
                unitOfWork.ComicRepo.CreateComicVolume(comic);
            }

            return comic;
        }

        private static void CreateComicActivity(Activity activity, Comic comic, string description, UnitOfWork unitOfWork)
        {
            var comicActivity = new ComicActivity() {
                Activity = activity,
                Comic = comic,
                Description = description
            };

            activity.ComicActivity = comicActivity;
            ActivityService.PairUpWithStartingActivity(activity, unitOfWork);

            unitOfWork.ComicRepo.CreateComicActivity(comicActivity);
        }

        //TODO add year to search
        public static Comic GetComicByTitle(string title, UnitOfWork unitOfWork)
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
                u.CategoryId == ActivityCategory.COMIC_ID
                && u.ComicActivity.Comic.Title == comic.Title
            );
        }
    }
}

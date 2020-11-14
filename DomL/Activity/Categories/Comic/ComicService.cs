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
    public class ComicService
    {
        public static void SaveFromRawSegments(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            segments[0] = "";
            var comicWindow = new ComicWindow(segments, activity, unitOfWork);

            if (ConfigurationManager.AppSettings["ShowCategoryWindows"] == "true") {
                comicWindow.ShowDialog();
            }

            var consolidated = new ConsolidatedComicDTO(comicWindow, activity);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        public static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            var consolidated = new ConsolidatedComicDTO(backupSegments);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        private static void SaveFromConsolidated(ConsolidatedComicDTO consolidated, UnitOfWork unitOfWork)
        {
            var type = MediaTypeService.GetByName(consolidated.Type, unitOfWork);
            var series = SeriesService.GetOrCreateByName(consolidated.SeriesName, unitOfWork);

            var comicVolume = GetOrUpdateOrCreateComicVolume(consolidated, series, unitOfWork);

            var activity = ActivityService.Create(consolidated, unitOfWork);
            CreateComicActivity(activity, comicVolume, consolidated.Description, unitOfWork);
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

        private static Comic GetOrUpdateOrCreateComicVolume(ConsolidatedComicDTO consolidated, Series series, UnitOfWork unitOfWork)
        {
            var comic = GetComicByTitle(consolidated.Title, unitOfWork);

            if (comic == null) {
                comic = new Comic() {
                    Series = series,
                    Title = consolidated.Title,
                    Author = consolidated.Author,
                    Type = consolidated.Type,
                    Publisher = consolidated.Publisher,
                    Year = int.Parse(consolidated.Year),
                    Score = consolidated.Score,
                };
                unitOfWork.ComicRepo.CreateComicVolume(comic);
            }

            return comic;
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
            var comicVolume = activity.ComicActivity.Comic;
            return previousStartingActivities.Where(u => 
                u.CategoryId == ActivityCategory.COMIC_ID
                && u.ComicActivity.Comic.Title == comicVolume.Title
            );
        }
    }
}

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
            var type = MediaTypeService.GetByName(consolidated.TypeName, unitOfWork);
            var series = SeriesService.GetOrCreateByName(consolidated.SeriesName, unitOfWork);
            var author = PersonService.GetOrCreateByName(consolidated.AuthorName, unitOfWork);
            var score = ScoreService.GetByValue(consolidated.ScoreValue, unitOfWork);

            var comicVolume = GetOrUpdateOrCreateComicVolume(series, consolidated.Chapters, author, type, score, unitOfWork);

            var activity = ActivityService.Create(consolidated, unitOfWork);
            CreateComicActivity(activity, comicVolume, consolidated.Description, unitOfWork);
        }

        private static void CreateComicActivity(Activity activity, ComicVolume comicVolume, string description, UnitOfWork unitOfWork)
        {
            var comicActivity = new ComicActivity() {
                Activity = activity,
                ComicVolume = comicVolume,
                Description = description
            };

            activity.ComicActivity = comicActivity;
            ActivityService.PairUpWithStartingActivity(activity, unitOfWork);

            unitOfWork.ComicRepo.CreateComicActivity(comicActivity);
        }

        private static ComicVolume GetOrUpdateOrCreateComicVolume(Series series, string chapters, Person author, MediaType type, Score score, UnitOfWork unitOfWork)
        {
            var comicVolume = GetComicVolumeBySeriesNameAndChapters(series.Name, chapters, unitOfWork);

            if (comicVolume == null) {
                comicVolume = new ComicVolume() {
                    Series = series,
                    Chapters = chapters,
                    Author = author,
                    Type = type,
                    Score = score,
                };
                unitOfWork.ComicRepo.CreateComicVolume(comicVolume);
            } else {
                comicVolume.Author = author ?? comicVolume.Author;
                comicVolume.Type = type ?? comicVolume.Type;
                comicVolume.Score = score ?? comicVolume.Score;
            }

            return comicVolume;
        }

        public static ComicVolume GetComicVolumeBySeriesNameAndChapters(string seriesName, string chapters, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(seriesName) || string.IsNullOrWhiteSpace(chapters)) {
                return null;
            }
            return unitOfWork.ComicRepo.GetComicVolumeBySeriesNameAndChapters(seriesName, chapters);
        }

        public static IEnumerable<Activity> GetStartingActivities(IQueryable<Activity> previousStartingActivities, Activity activity)
        {
            var comicVolume = activity.ComicActivity.ComicVolume;
            return previousStartingActivities.Where(u => 
                u.CategoryId == ActivityCategory.COMIC_ID
                && u.ComicActivity.ComicVolume.Series.Name == comicVolume.Series.Name && u.ComicActivity.ComicVolume.Chapters == comicVolume.Chapters
            );
        }
    }
}

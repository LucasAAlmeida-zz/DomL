﻿using DomL.Business.DTOs;
using DomL.Business.Entities;
using DomL.Presentation;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace DomL.Business.Services
{
    public class ComicService
    {
        public static void SaveFromRawSegments(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            // COMIC (Classification); Series Name; Chapters; (Author Name); (Media Type Name); (Score); (Description)
            segments[0] = "";
            var comicWindow = new ComicWindow(segments, activity, unitOfWork);

            if (ConfigurationManager.AppSettings["ShowCategoryWindows"] == "true") {
                comicWindow.ShowDialog();
            }

            var seriesName = comicWindow.SeriesCB.Text;
            var chapters = comicWindow.ChaptersCB.Text;
            var authorName = comicWindow.AuthorCB.Text;
            var typeName = comicWindow.TypeCB.Text;
            var score = (!string.IsNullOrWhiteSpace(comicWindow.ScoreCB.Text)) ? comicWindow.ScoreCB.Text : null;
            var description = (!string.IsNullOrWhiteSpace(comicWindow.DescriptionCB.Text)) ? comicWindow.DescriptionCB.Text : null;
                
            Series series = SeriesService.GetOrCreateByName(seriesName, unitOfWork);
            Person author = PersonService.GetOrCreateByName(authorName, unitOfWork);
            MediaType type = MediaTypeService.GetOrCreateByName(typeName, unitOfWork);

            ComicVolume comicVolume = GetOrUpdateOrCreateComicVolume(series, chapters, author, type, score, unitOfWork);
            CreateComicActivity(activity, comicVolume, description, unitOfWork);
        }

        private static void CreateComicActivity(Activity activity, ComicVolume comicVolume, string description, UnitOfWork unitOfWork)
        {
            var comicActivity = new ComicActivity() {
                Activity = activity,
                ComicVolume = comicVolume,
                Description = description
            };

            activity.ComicActivity = comicActivity;
            activity.PairUpActivity(unitOfWork);

            unitOfWork.ComicRepo.CreateComicActivity(comicActivity);
        }

        private static ComicVolume GetOrUpdateOrCreateComicVolume(Series series, string chapters, Person author, MediaType type, string score, UnitOfWork unitOfWork)
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
                u.CategoryId == ActivityCategory.COMIC
                && u.ComicActivity.ComicVolume.Series.Name == comicVolume.Series.Name && u.ComicActivity.ComicVolume.Chapters == comicVolume.Chapters
            );
        }
    }
}
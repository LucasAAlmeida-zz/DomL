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
            var scoreValue = comicWindow.ScoreCB.Text;
            var description = (!string.IsNullOrWhiteSpace(comicWindow.DescriptionCB.Text)) ? comicWindow.DescriptionCB.Text : null;
                
            var type = MediaTypeService.GetByName(typeName, unitOfWork);
            var series = SeriesService.GetOrCreateByName(seriesName, unitOfWork);
            var author = PersonService.GetOrCreateByName(authorName, unitOfWork);
            var score = ScoreService.GetByValue(scoreValue, unitOfWork);

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

        public static void RestoreFromFile(string fileDir)
        {
            using (var reader = new StreamReader(fileDir + "Comic.txt")) {
                string line = "";
                while ((line = reader.ReadLine()) != null) {
                    if (string.IsNullOrWhiteSpace(line)) {
                        continue;
                    }

                    var segments = Regex.Split(line, "\t");

                    // Date Start; Date Finish; Series Name; Chapters; (Author Name); (Media Type Name); (Score); (Description)
                    var dateStart = segments[0];
                    var dateFinish = segments[1];
                    var seriesName = segments[2];
                    var chapters = segments[3];
                    var authorName = segments[4] != "-" ? segments[4] : null;
                    var typeName = segments[5] != "-" ? segments[5] : null;
                    var scoreValue = segments[6] != "-" ? segments[6] : null;
                    var description = segments[7] != "-" ? segments[7] : null;

                    var originalLine = seriesName + "; " + chapters;
                    originalLine = (!string.IsNullOrWhiteSpace(authorName)) ? originalLine + "; " + authorName : originalLine;
                    originalLine = (!string.IsNullOrWhiteSpace(typeName)) ? originalLine + "; " + typeName : originalLine;
                    originalLine = (!string.IsNullOrWhiteSpace(scoreValue)) ? originalLine + "; " + scoreValue : originalLine;
                    originalLine = (!string.IsNullOrWhiteSpace(description)) ? originalLine + "; " + description : originalLine;

                    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                        var series = SeriesService.GetOrCreateByName(seriesName, unitOfWork);
                        var author = PersonService.GetOrCreateByName(authorName, unitOfWork);
                        var type = MediaTypeService.GetByName(typeName, unitOfWork);
                        var score = ScoreService.GetByValue(scoreValue, unitOfWork);

                        ComicVolume comicVolume = GetOrUpdateOrCreateComicVolume(series, chapters, author, type, score, unitOfWork);

                        var statusStart = unitOfWork.ActivityRepo.GetStatusById(ActivityStatus.START);
                        var statusFinish = unitOfWork.ActivityRepo.GetStatusById(ActivityStatus.FINISH);
                        var statusSingle = unitOfWork.ActivityRepo.GetStatusById(ActivityStatus.SINGLE);

                        var category = unitOfWork.ActivityRepo.GetCategoryById(ActivityCategory.COMIC_ID);

                        if (!dateStart.StartsWith("--") && dateStart != dateFinish) {
                            Activity activityStart = null;
                            Activity activityFinish = null;
                            if (!dateStart.StartsWith("??")) {
                                var date = DateTime.ParseExact(dateStart, "dd/MM/yy", null);
                                activityStart = ActivityService.Create(date, 0, statusStart, category, null, "COMIC Start; " + originalLine, unitOfWork);
                                CreateComicActivity(activityStart, comicVolume, description, unitOfWork);
                            }
                            if (!dateFinish.StartsWith("??")) {
                                var date = DateTime.ParseExact(dateFinish, "dd/MM/yy", null);
                                activityFinish = ActivityService.Create(date, 0, statusFinish, category, null, "COMIC Finish; " + originalLine, unitOfWork);
                                CreateComicActivity(activityFinish, comicVolume, description, unitOfWork);
                            }

                            if (activityStart != null && activityFinish != null) {
                                activityStart.ComicActivity.Description = null;
                                activityStart.PairedActivity = activityFinish;
                                unitOfWork.Complete();
                                activityFinish.PairedActivity = activityStart;
                            }
                        } else {
                            var date = DateTime.ParseExact(dateFinish, "dd/MM/yy", null);
                            var activity = ActivityService.Create(date, 0, statusSingle, category, null, "COMIC; " + originalLine, unitOfWork);
                            CreateComicActivity(activity, comicVolume, description, unitOfWork);
                        }

                        unitOfWork.Complete();
                    }
                }
            }
        }
    }
}

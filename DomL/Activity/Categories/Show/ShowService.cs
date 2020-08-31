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
        public static void SaveFromRawSegments(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            // SHOW (Classification); Series Name; Season; (Director Name); (Media Type Name); (Score); (Description)
            segments[0] = "";
            var showWindow = new ShowWindow(segments, activity, unitOfWork);

            if (ConfigurationManager.AppSettings["ShowCategoryWindows"] == "true") {
                showWindow.ShowDialog();
            }

            var seriesName = showWindow.SeriesCB.Text;
            var season = showWindow.SeasonCB.Text;
            var directorName = showWindow.DirectorCB.Text;
            var typeName = showWindow.TypeCB.Text;
            var scoreValue = showWindow.ScoreCB.Text;
            var description = (!string.IsNullOrWhiteSpace(showWindow.DescriptionCB.Text)) ? showWindow.DescriptionCB.Text : null;
                
            var series = SeriesService.GetOrCreateByName(seriesName, unitOfWork);
            var director = PersonService.GetOrCreateByName(directorName, unitOfWork);
            var type = MediaTypeService.GetByName(typeName, unitOfWork);
            var score = ScoreService.GetByValue(scoreValue, unitOfWork);

            ShowSeason showSeason = GetOrUpdateOrCreateShowSeason(series, season, director, type, score, unitOfWork);
            CreateShowActivity(activity, showSeason, description, unitOfWork);
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

        public static void RestoreFromFile(string fileDir)
        {
            using (var reader = new StreamReader(fileDir + "Show.txt")) {
                string line = "";
                while ((line = reader.ReadLine()) != null) {
                    if (string.IsNullOrWhiteSpace(line)) {
                        continue;
                    }

                    var segments = Regex.Split(line, "\t");

                    // Date Start; Date Finish; Series Name; Season; (Director Name); (Media Type Name); (Score); (Description)
                    var dateStart = segments[0];
                    var dateFinish = segments[1];
                    var seriesName = segments[2];
                    var season = segments[3];
                    var directorName = segments[4] != "-" ? segments[4] : null;
                    var typeName = segments[5] != "-" ? segments[5] : null;
                    var scoreValue = segments[6] != "-" ? segments[6] : null;
                    var description = segments[7] != "-" ? segments[7] : null;

                    var originalLine = seriesName + "; " + season;
                    originalLine = (!string.IsNullOrWhiteSpace(directorName)) ? originalLine + "; " + directorName : originalLine;
                    originalLine = (!string.IsNullOrWhiteSpace(typeName)) ? originalLine + "; " + typeName : originalLine;
                    originalLine = (!string.IsNullOrWhiteSpace(scoreValue)) ? originalLine + "; " + scoreValue : originalLine;
                    originalLine = (!string.IsNullOrWhiteSpace(description)) ? originalLine + "; " + description : originalLine;

                    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                        var series = SeriesService.GetOrCreateByName(seriesName, unitOfWork);
                        var director = PersonService.GetOrCreateByName(directorName, unitOfWork);
                        var type = MediaTypeService.GetByName(typeName, unitOfWork);
                        var score = ScoreService.GetByValue(scoreValue, unitOfWork);

                        ShowSeason showSeason = GetOrUpdateOrCreateShowSeason(series, season, director, type, score, unitOfWork);

                        var statusStart = unitOfWork.ActivityRepo.GetStatusById(ActivityStatus.START);
                        var statusFinish = unitOfWork.ActivityRepo.GetStatusById(ActivityStatus.FINISH);
                        var statusSingle = unitOfWork.ActivityRepo.GetStatusById(ActivityStatus.SINGLE);

                        var category = unitOfWork.ActivityRepo.GetCategoryById(ActivityCategory.SHOW_ID);

                        if (!dateStart.StartsWith("--") && dateStart != dateFinish) {
                            Activity activityStart = null;
                            Activity activityFinish = null;
                            if (!dateStart.StartsWith("??")) {
                                var date = DateTime.ParseExact(dateStart, "dd/MM/yy", null);
                                activityStart = ActivityService.Create(date, 0, statusStart, category, null, "SHOW Start; " + originalLine, unitOfWork);
                                CreateShowActivity(activityStart, showSeason, description, unitOfWork);
                            }
                            if (!dateFinish.StartsWith("??")) {
                                var date = DateTime.ParseExact(dateFinish, "dd/MM/yy", null);
                                activityFinish = ActivityService.Create(date, 0, statusFinish, category, null, "SHOW Finish; " + originalLine, unitOfWork);
                                CreateShowActivity(activityFinish, showSeason, description, unitOfWork);
                            }

                            if (activityStart != null && activityFinish != null) {
                                activityStart.ShowActivity.Description = null;
                                activityStart.PairedActivity = activityFinish;
                                unitOfWork.Complete();
                                activityFinish.PairedActivity = activityStart;
                            }
                        } else {
                            var date = DateTime.ParseExact(dateFinish, "dd/MM/yy", null);
                            var activity = ActivityService.Create(date, 0, statusSingle, category, null, "SHOW; " + originalLine, unitOfWork);
                            CreateShowActivity(activity, showSeason, description, unitOfWork);
                        }

                        unitOfWork.Complete();
                    }
                }
            }
        }
    }
}

using DomL.Business.Entities;
using DomL.Presentation;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

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
            var score = (!string.IsNullOrWhiteSpace(showWindow.ScoreCB.Text)) ? showWindow.ScoreCB.Text : null;
            var description = (!string.IsNullOrWhiteSpace(showWindow.DescriptionCB.Text)) ? showWindow.DescriptionCB.Text : null;
                
            Series series = SeriesService.GetOrCreateByName(seriesName, unitOfWork);
            Person director = PersonService.GetOrCreateByName(directorName, unitOfWork);
            MediaType type = MediaTypeService.GetOrCreateByName(typeName, unitOfWork);

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

        private static ShowSeason GetOrUpdateOrCreateShowSeason(Series series, string season, Person director, MediaType type, string score, UnitOfWork unitOfWork)
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
    }
}

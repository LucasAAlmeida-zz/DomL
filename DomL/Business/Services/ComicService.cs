using DomL.Business.DTOs;
using DomL.Business.Entities;
using DomL.Presentation;
using System.Collections.Generic;
using System.Linq;

namespace DomL.Business.Services
{
    public class ComicService
    {
        public static void SaveFromRawSegments(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            // COMIC (Classification); Series Name; Chapters; (Author Name); (Media Type Name); (Score); (Description)
            segments[0] = "";
            var comicWindow = new ComicWindow(segments);
            //comicWindow.ShowDialog();

            var seriesName = (string) comicWindow.SeriesCB.SelectedItem;
            var chapters = (string) comicWindow.ChaptersCB.SelectedItem;
            var authorName = (string) comicWindow.AuthorCB.SelectedItem;
            var typeName = (string) comicWindow.TypeCB.SelectedItem;
            var score = (string) comicWindow.ScoreCB.SelectedItem;
            var description = (string) comicWindow.DescriptionCB.SelectedItem;
                
            Series series = SeriesService.GetOrCreateByName(seriesName, unitOfWork);
            Person author = PersonService.GetOrCreateByName(authorName, unitOfWork);
            MediaType type = MediaTypeService.GetOrCreateByName(typeName, unitOfWork);

            ComicVolume comicVolume = GetOrCreateComicVolume(series, chapters, author, type, score, unitOfWork);
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
            activity.PairActivity(unitOfWork);

            unitOfWork.ComicRepo.CreateComicActivity(comicActivity);
        }

        private static ComicVolume GetOrCreateComicVolume(Series series, string chapters, Person author, MediaType type, string score, UnitOfWork unitOfWork)
        {
            var comicVolume = unitOfWork.ComicRepo.GetComicVolumeByChapters(chapters);

            if (comicVolume == null) {
                comicVolume = new ComicVolume() {
                    Series = series,
                    Chapters = chapters,
                    Author = author,
                    Type = type,
                    Score = score,
                };
                unitOfWork.ComicRepo.Add(comicVolume);
            }

            return comicVolume;
        }

        public static string GetString(Activity activity, int kindOfString)
        {
            var consolidatedInfo = new ConsolidatedComicActivityDTO(activity);
            switch (kindOfString) {
                case 0:     return consolidatedInfo.GetInfoForMonthRecap();
                case 1:     return consolidatedInfo.GetInfoForYearRecap();
                case 2:     return consolidatedInfo.GetInfoForYearRecap();
                default:    return "";
            }
        }

        public static IEnumerable<Activity> GetStartingActivity(List<Activity> previousStartingActivities, Activity activity)
        {
            var comicVolume = activity.ComicActivity.ComicVolume;
            return previousStartingActivities.Where(u => 
                u.CategoryId == ActivityCategory.COMIC
                && IsSameComicVolume(u.ComicActivity.ComicVolume, comicVolume)
            );
        }

        private static bool IsSameComicVolume(ComicVolume comicVolume1, ComicVolume comicVolume2)
        {
            return comicVolume1.Series.Name == comicVolume2.Series.Name && comicVolume1.Chapters == comicVolume2.Chapters;
        }
    }
}

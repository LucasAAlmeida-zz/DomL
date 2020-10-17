using DomL.Business.DTOs;
using DomL.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DomL.Business.Services
{
    public class ActivityService
    {
        public static Activity Create(DateTime date, int dayOrder, ActivityStatus status, ActivityCategory category, ActivityBlock activityBlock, string originalLine, UnitOfWork unitOfWork)
        {
            var activity = new Activity() {
                Date = date,
                DayOrder = dayOrder,
                Status = status,
                Category = category,
                ActivityBlock = activityBlock,
                OriginalLine = originalLine
            };

            unitOfWork.ActivityRepo.Add(activity);
            return activity;
        }

        public static ActivityBlock ChangeActivityBlock(string rawLine, UnitOfWork unitOfWork)
        {
            if (rawLine == "<END>") {
                return null;
            }

            var newBlockName = rawLine.Substring(1, rawLine.Length - 2);
            var activityBlock = new ActivityBlock {
                Name = newBlockName
            };
            unitOfWork.ActivityRepo.CreateActivityBlock(activityBlock);
            return activityBlock;
        }

        public static ActivityCategory GetCategory(string rawLine, UnitOfWork unitOfWork)
        {
            var segments = Regex.Split(rawLine, "; ");
            
            if (segments.Count() == 1) {
                return unitOfWork.ActivityRepo.GetCategoryByName("EVENT");
            }

            var categoryName = Regex.Split(segments[0], " ")[0];
            var category = unitOfWork.ActivityRepo.GetCategoryByName(categoryName);
            return category ?? unitOfWork.ActivityRepo.GetCategoryByName("EVENT");
        }

        public static ActivityStatus GetStatus(string rawLine, UnitOfWork unitOfWork)
        {
            var segments = Regex.Split(rawLine, "; ");
            segments = Regex.Split(segments[0], " ");

            string statusName;
            if (segments.Length == 1) {
                statusName = "SINGLE";
            } else if (IsStringFinish(segments[1])) {
                statusName = "FINISH";
            } else if (IsStringStart(segments[1])) {
                statusName = "START";
            } else {
                statusName = "SINGLE";
            }
            return unitOfWork.ActivityRepo.GetStatusByName(statusName);
        }

        private static bool IsStringFinish(string word)
        {
            return word.ToLower() == "finish";
        }

        private static bool IsStringStart(string word)
        {
            return word.ToLower() == "start";
        }

        public static void SaveFromRawLine(Activity activity, string rawLine, UnitOfWork unitOfWork)
        {
            var segments = Regex.Split(rawLine, "; ");
            switch (activity.Category.Id) {
                case ActivityCategory.AUTO_ID:     AutoService.SaveFromRawSegments(segments, activity, unitOfWork);        break;
                case ActivityCategory.BOOK_ID:     BookService.SaveFromRawSegments(segments, activity, unitOfWork);        break;
                case ActivityCategory.COMIC_ID:    ComicService.SaveFromRawSegments(segments, activity, unitOfWork);       break;
                case ActivityCategory.COURSE_ID:   CourseService.SaveFromRawSegments(segments, activity, unitOfWork);      break;
                case ActivityCategory.DOOM_ID:     DoomService.SaveFromRawSegments(segments, activity, unitOfWork);        break;
                case ActivityCategory.EVENT_ID:    EventService.SaveFromRawSegments(segments, activity, unitOfWork);       break;
                case ActivityCategory.GAME_ID:     GameService.SaveFromRawSegments(segments, activity, unitOfWork);        break;
                case ActivityCategory.GIFT_ID:     GiftService.SaveFromRawSegments(segments, activity, unitOfWork);        break;
                case ActivityCategory.HEALTH_ID:   HealthService.SaveFromRawSegments(segments, activity, unitOfWork);      break;
                case ActivityCategory.MOVIE_ID:    MovieService.SaveFromRawSegments(segments, activity, unitOfWork);       break;
                case ActivityCategory.PET_ID:      PetService.SaveFromRawSegments(segments, activity, unitOfWork);         break;
                case ActivityCategory.MEET_ID:     MeetService.SaveFromRawSegments(segments, activity, unitOfWork);        break;
                case ActivityCategory.PLAY_ID:     PlayService.SaveFromRawSegments(segments, activity, unitOfWork);        break;
                case ActivityCategory.PURCHASE_ID: PurchaseService.SaveFromRawSegments(segments, activity, unitOfWork);    break;
                case ActivityCategory.SHOW_ID:     ShowService.SaveFromRawSegments(segments, activity, unitOfWork);        break;
                case ActivityCategory.TRAVEL_ID:   TravelService.SaveFromRawSegments(segments, activity, unitOfWork);      break;
                case ActivityCategory.WORK_ID:     WorkService.SaveFromRawSegments(segments, activity, unitOfWork);        break;
            }
        }

        public static void PairUpWithStartingActivity(Activity activity, UnitOfWork unitOfWork)
        {
            if (activity.Status.Id != ActivityStatus.FINISH) {
                return;
            }

            var startingActivity = GetStartingActivity(activity, unitOfWork);

            if (startingActivity != null) {
                activity.PairedActivity = startingActivity;
                startingActivity.PairedActivity = activity;
            }
        }

        private static Activity GetStartingActivity(Activity activity, UnitOfWork unitOfWork)
        {
            var psa = unitOfWork.ActivityRepo.GetPreviousStartingActivities(activity.Date);

            IEnumerable<Activity> pcsa = null; // Previous Category Starting Activities
            switch (activity.Category.Id) {
                case ActivityCategory.AUTO_ID:     pcsa = AutoService.GetStartingActivities(psa, activity);      break;
                case ActivityCategory.BOOK_ID:     pcsa = BookService.GetStartingActivities(psa, activity);      break;
                case ActivityCategory.COMIC_ID:    pcsa = ComicService.GetStartingActivities(psa, activity);     break;
                case ActivityCategory.COURSE_ID:   pcsa = CourseService.GetStartingActivities(psa, activity);     break;
                case ActivityCategory.DOOM_ID:     pcsa = DoomService.GetStartingActivities(psa, activity);      break;
                case ActivityCategory.GAME_ID:     pcsa = GameService.GetStartingActivities(psa, activity);      break;
                case ActivityCategory.HEALTH_ID:   pcsa = HealthService.GetStartingActivities(psa, activity);    break;
                case ActivityCategory.MOVIE_ID:    pcsa = MovieService.GetStartingActivities(psa, activity);     break;
                case ActivityCategory.PET_ID:      pcsa = PetService.GetStartingActivities(psa, activity);       break;
                case ActivityCategory.SHOW_ID:     pcsa = ShowService.GetStartingActivities(psa, activity);      break;
                case ActivityCategory.WORK_ID:     pcsa = WorkService.GetStartingActivities(psa, activity);      break;
            }
            return pcsa.OrderByDescending(u => u.Date).FirstOrDefault();
        }

        public static string GetInfoForMonthRecap(Activity activity)
        {
            var consolidated = new ConsolidatedActivityDTO(activity);

            if (activity.CategoryId == ActivityCategory.EVENT_ID && activity.ActivityBlock == null && !activity.EventActivity.IsImportant) {
                return "";
            }

            return consolidated.GetInfoForMonthRecap();
        }

        public static string GetInfoForYearRecap(Activity activity)
        {
            if (activity.StatusId == ActivityStatus.START && activity.PairedActivity != null) {
                return "";
            }

            switch (activity.Category.Id) {
                case ActivityCategory.AUTO_ID:     return new ConsolidatedAutoDTO(activity).GetInfoForYearRecap();
                case ActivityCategory.BOOK_ID:     return new ConsolidatedBookDTO(activity).GetInfoForYearRecap();
                case ActivityCategory.COMIC_ID:    return new ConsolidatedComicDTO(activity).GetInfoForYearRecap();
                case ActivityCategory.COURSE_ID:   return new ConsolidatedCourseDTO(activity).GetInfoForYearRecap();
                case ActivityCategory.DOOM_ID:     return new ConsolidatedDoomDTO(activity).GetInfoForYearRecap();
                case ActivityCategory.EVENT_ID:    return new ConsolidatedEventDTO(activity).GetInfoForYearRecap();
                case ActivityCategory.GAME_ID:     return new ConsolidatedGameDTO(activity).GetInfoForYearRecap();
                case ActivityCategory.GIFT_ID:     return new ConsolidatedGiftDTO(activity).GetInfoForYearRecap();
                case ActivityCategory.HEALTH_ID:   return new ConsolidatedHealthDTO(activity).GetInfoForYearRecap();
                case ActivityCategory.MOVIE_ID:    return new ConsolidatedMovieDTO(activity).GetInfoForYearRecap();
                case ActivityCategory.PET_ID:      return new ConsolidatedPetDTO(activity).GetInfoForYearRecap();
                case ActivityCategory.MEET_ID:     return new ConsolidatedMeetDTO(activity).GetInfoForYearRecap();
                case ActivityCategory.PLAY_ID:     return new ConsolidatedPlayDTO(activity).GetInfoForYearRecap();
                case ActivityCategory.PURCHASE_ID: return new ConsolidatedPurchaseDTO(activity).GetInfoForYearRecap();
                case ActivityCategory.SHOW_ID:     return new ConsolidatedShowDTO(activity).GetInfoForYearRecap();
                case ActivityCategory.TRAVEL_ID:   return new ConsolidatedTravelDTO(activity).GetInfoForYearRecap();
                case ActivityCategory.WORK_ID:     return new ConsolidatedWorkDTO(activity).GetInfoForYearRecap();
            }
            return "";
        }

        public static string GetInfoForBackup(Activity activity)
        {
            switch (activity.Category.Id) {
                case ActivityCategory.AUTO_ID:     return new ConsolidatedAutoDTO(activity).GetInfoForBackup();
                case ActivityCategory.BOOK_ID:     return new ConsolidatedBookDTO(activity).GetInfoForBackup();
                case ActivityCategory.COMIC_ID:    return new ConsolidatedComicDTO(activity).GetInfoForBackup();
                case ActivityCategory.COURSE_ID:   return new ConsolidatedCourseDTO(activity).GetInfoForBackup();
                case ActivityCategory.DOOM_ID:     return new ConsolidatedDoomDTO(activity).GetInfoForBackup();
                case ActivityCategory.EVENT_ID:    return new ConsolidatedEventDTO(activity).GetInfoForBackup();
                case ActivityCategory.GAME_ID:     return new ConsolidatedGameDTO(activity).GetInfoForBackup();
                case ActivityCategory.GIFT_ID:     return new ConsolidatedGiftDTO(activity).GetInfoForBackup();
                case ActivityCategory.HEALTH_ID:   return new ConsolidatedHealthDTO(activity).GetInfoForBackup();
                case ActivityCategory.MOVIE_ID:    return new ConsolidatedMovieDTO(activity).GetInfoForBackup();
                case ActivityCategory.PET_ID:      return new ConsolidatedPetDTO(activity).GetInfoForBackup();
                case ActivityCategory.MEET_ID:     return new ConsolidatedMeetDTO(activity).GetInfoForBackup();
                case ActivityCategory.PLAY_ID:     return new ConsolidatedPlayDTO(activity).GetInfoForBackup();
                case ActivityCategory.PURCHASE_ID: return new ConsolidatedPurchaseDTO(activity).GetInfoForBackup();
                case ActivityCategory.SHOW_ID:     return new ConsolidatedShowDTO(activity).GetInfoForBackup();
                case ActivityCategory.TRAVEL_ID:   return new ConsolidatedTravelDTO(activity).GetInfoForBackup();
                case ActivityCategory.WORK_ID:     return new ConsolidatedWorkDTO(activity).GetInfoForBackup();
            }
            return "";
        }
    }
}
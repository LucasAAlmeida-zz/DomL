using DomL.Business.DTOs;
using DomL.Business.Entities;
using DomL.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DomL.Business.Services
{
    public class ActivityService
    {
        public static Activity Create(ActivityConsolidatedDTO consolidated, UnitOfWork unitOfWork)
        {
            var date = DateTime.ParseExact(consolidated.Date, "yyyy/MM/dd", null);
            var dayOrder = int.Parse(consolidated.DayOrder);

            var status = GetStatusByName(consolidated.StatusName, unitOfWork);
            var category = GetCategoryByName(consolidated.CategoryName, unitOfWork);
            var block = CreateOrGetBlockByName(consolidated.BlockName, unitOfWork);

            var activity = new Activity() {
                Date = date,
                DayOrder = dayOrder,
                Status = status,
                Category = category,
                ActivityBlock = block,
                OriginalLine = consolidated.OriginalLine
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

        private static ActivityBlock CreateOrGetBlockByName(string blockName, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(blockName)) {
                return null;
            }

            var block = GetActivityBlockByName(blockName, unitOfWork);

            if (block == null) {
                block = CreateActivityBlock(blockName, unitOfWork);
            }

            return block;
        }

        private static ActivityBlock GetActivityBlockByName(string blockName, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(blockName)) {
                return null;
            }
            return unitOfWork.ActivityRepo.GetActivityBlockByName(blockName);
        }

        private static ActivityBlock CreateActivityBlock(string blockName, UnitOfWork unitOfWork)
        {
            var block = new ActivityBlock() {
                Name = blockName,
            };
            unitOfWork.ActivityRepo.CreateActivityBlock(block);
            return block;
        }

        private static ActivityCategory GetCategoryByName(string categoryName, UnitOfWork unitOfWork)
        {
            return unitOfWork.ActivityRepo.GetCategoryByName(categoryName);
        }

        private static ActivityStatus GetStatusByName(string statusName, UnitOfWork unitOfWork)
        {
            statusName = (statusName != "-") ? statusName : "Single";
            return unitOfWork.ActivityRepo.GetStatusByName(statusName);
        }

        public static ActivityCategory GetCategory(string rawLine, UnitOfWork unitOfWork)
        {
            var segments = Regex.Split(rawLine, "; ");
            
            if (segments.Count() == 1) {
                return unitOfWork.ActivityRepo.GetCategoryByName("EVENT");
            }

            var categoryName = Regex.Split(segments[0], " ")[0];
            var category = GetCategoryByName(categoryName, unitOfWork);
            return category ?? GetCategoryByName("EVENT", unitOfWork);
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
            return GetStatusByName(statusName, unitOfWork);
        }

        private static bool IsStringFinish(string word)
        {
            return word.ToLower() == "finish";
        }

        private static bool IsStringStart(string word)
        {
            return word.ToLower() == "start";
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

        public static void SaveFromRawSegments(string[] rawSegments, Activity activity, UnitOfWork unitOfWork)
        {
            switch (activity.Category.Id) {
                case ActivityCategory.AUTO_ID:     AutoService.SaveFromRawSegments(rawSegments, activity, unitOfWork);        break;
                case ActivityCategory.BOOK_ID:     BookService.SaveFromRawSegments(rawSegments, activity, unitOfWork);        break;
                case ActivityCategory.COMIC_ID:    ComicService.SaveFromRawSegments(rawSegments, activity, unitOfWork);       break;
                case ActivityCategory.COURSE_ID:   CourseService.SaveFromRawSegments(rawSegments, activity, unitOfWork);      break;
                case ActivityCategory.DOOM_ID:     DoomService.SaveFromRawSegments(rawSegments, activity, unitOfWork);        break;
                case ActivityCategory.EVENT_ID:    EventService.SaveFromRawSegments(rawSegments, activity, unitOfWork);       break;
                case ActivityCategory.GAME_ID:     GameService.SaveFromRawSegments(rawSegments, activity, unitOfWork);        break;
                case ActivityCategory.GIFT_ID:     GiftService.SaveFromRawSegments(rawSegments, activity, unitOfWork);        break;
                case ActivityCategory.HEALTH_ID:   HealthService.SaveFromRawSegments(rawSegments, activity, unitOfWork);      break;
                case ActivityCategory.MOVIE_ID:    MovieService.SaveFromRawSegments(rawSegments, activity, unitOfWork);       break;
                case ActivityCategory.PET_ID:      PetService.SaveFromRawSegments(rawSegments, activity, unitOfWork);         break;
                case ActivityCategory.MEET_ID:     MeetService.SaveFromRawSegments(rawSegments, activity, unitOfWork);        break;
                case ActivityCategory.PLAY_ID:     PlayService.SaveFromRawSegments(rawSegments, activity, unitOfWork);        break;
                case ActivityCategory.PURCHASE_ID: PurchaseService.SaveFromRawSegments(rawSegments, activity, unitOfWork);    break;
                case ActivityCategory.SHOW_ID:     ShowService.SaveFromRawSegments(rawSegments, activity, unitOfWork);        break;
                case ActivityCategory.TRAVEL_ID:   TravelService.SaveFromRawSegments(rawSegments, activity, unitOfWork);      break;
                case ActivityCategory.WORK_ID:     WorkService.SaveFromRawSegments(rawSegments, activity, unitOfWork);        break;
            }
        }

        public static void SaveFromBackupSegments(string[] backupSegments, ActivityCategory category, UnitOfWork unitOfWork)
        {
            switch (category.Id) {
                case ActivityCategory.AUTO_ID:     AutoService.SaveFromBackupSegments(backupSegments, unitOfWork);      break;
                case ActivityCategory.BOOK_ID:     BookService.SaveFromBackupSegments(backupSegments, unitOfWork);      break;
                case ActivityCategory.COMIC_ID:    ComicService.SaveFromBackupSegments(backupSegments, unitOfWork);     break;
                case ActivityCategory.COURSE_ID:   CourseService.SaveFromBackupSegments(backupSegments, unitOfWork);    break;
                case ActivityCategory.DOOM_ID:     DoomService.SaveFromBackupSegments(backupSegments, unitOfWork);      break;
                case ActivityCategory.EVENT_ID:    EventService.SaveFromBackupSegments(backupSegments, unitOfWork);     break;
                case ActivityCategory.GAME_ID:     GameService.SaveFromBackupSegments(backupSegments, unitOfWork);      break;
                case ActivityCategory.GIFT_ID:     GiftService.SaveFromBackupSegments(backupSegments, unitOfWork);      break;
                case ActivityCategory.HEALTH_ID:   HealthService.SaveFromBackupSegments(backupSegments, unitOfWork);    break;
                case ActivityCategory.MOVIE_ID:    MovieService.SaveFromBackupSegments(backupSegments, unitOfWork);     break;
                case ActivityCategory.PET_ID:      PetService.SaveFromBackupSegments(backupSegments, unitOfWork);       break;
                case ActivityCategory.MEET_ID:     MeetService.SaveFromBackupSegments(backupSegments, unitOfWork);      break;
                case ActivityCategory.PLAY_ID:     PlayService.SaveFromBackupSegments(backupSegments, unitOfWork);      break;
                case ActivityCategory.PURCHASE_ID: PurchaseService.SaveFromBackupSegments(backupSegments, unitOfWork);  break;
                case ActivityCategory.SHOW_ID:     ShowService.SaveFromBackupSegments(backupSegments, unitOfWork);      break;
                case ActivityCategory.TRAVEL_ID:   TravelService.SaveFromBackupSegments(backupSegments, unitOfWork);    break;
                case ActivityCategory.WORK_ID:     WorkService.SaveFromBackupSegments(backupSegments, unitOfWork);      break;
            }
        }

        public static string GetInfoForMonthRecap(Activity activity)
        {
            var consolidated = new ActivityConsolidatedDTO(activity);

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
                case ActivityCategory.AUTO_ID:     return new AutoConsolidatedDTO(activity).GetInfoForYearRecap();
                case ActivityCategory.BOOK_ID:     return new BookConsolidatedDTO(activity).GetInfoForYearRecap();
                case ActivityCategory.COMIC_ID:    return new ComicConsolidatedDTO(activity).GetInfoForYearRecap();
                case ActivityCategory.COURSE_ID:   return new CourseConsolidatedDTO(activity).GetInfoForYearRecap();
                case ActivityCategory.DOOM_ID:     return new DoomConsolidatedDTO(activity).GetInfoForYearRecap();
                case ActivityCategory.EVENT_ID:    return new EventConsolidatedDTO(activity).GetInfoForYearRecap();
                case ActivityCategory.GAME_ID:     return new ConsolidatedGameDTO(activity).GetInfoForYearRecap();
                case ActivityCategory.GIFT_ID:     return new GiftConsolidatedDTO(activity).GetInfoForYearRecap();
                case ActivityCategory.HEALTH_ID:   return new HealthConsolidatedDTO(activity).GetInfoForYearRecap();
                case ActivityCategory.MOVIE_ID:    return new MovieConsolidatedDTO(activity).GetInfoForYearRecap();
                case ActivityCategory.PET_ID:      return new PetConsolidatedDTO(activity).GetInfoForYearRecap();
                case ActivityCategory.MEET_ID:     return new MeetConsolidatedDTO(activity).GetInfoForYearRecap();
                case ActivityCategory.PLAY_ID:     return new PlayConsolidatedDTO(activity).GetInfoForYearRecap();
                case ActivityCategory.PURCHASE_ID: return new PurchaseConsolidatedDTO(activity).GetInfoForYearRecap();
                case ActivityCategory.SHOW_ID:     return new ShowConsolidatedDTO(activity).GetInfoForYearRecap();
                case ActivityCategory.TRAVEL_ID:   return new ConsolidatedTravelDTO(activity).GetInfoForYearRecap();
                case ActivityCategory.WORK_ID:     return new ConsolidatedWorkDTO(activity).GetInfoForYearRecap();
            }
            return "";
        }

        public static string GetInfoForBackup(Activity activity)
        {
            switch (activity.Category.Id) {
                case ActivityCategory.AUTO_ID:     return new AutoConsolidatedDTO(activity).GetInfoForBackup();
                case ActivityCategory.BOOK_ID:     return new BookConsolidatedDTO(activity).GetInfoForBackup();
                case ActivityCategory.COMIC_ID:    return new ComicConsolidatedDTO(activity).GetInfoForBackup();
                case ActivityCategory.COURSE_ID:   return new CourseConsolidatedDTO(activity).GetInfoForBackup();
                case ActivityCategory.DOOM_ID:     return new DoomConsolidatedDTO(activity).GetInfoForBackup();
                case ActivityCategory.EVENT_ID:    return new EventConsolidatedDTO(activity).GetInfoForBackup();
                case ActivityCategory.GAME_ID:     return new ConsolidatedGameDTO(activity).GetInfoForBackup();
                case ActivityCategory.GIFT_ID:     return new GiftConsolidatedDTO(activity).GetInfoForBackup();
                case ActivityCategory.HEALTH_ID:   return new HealthConsolidatedDTO(activity).GetInfoForBackup();
                case ActivityCategory.MOVIE_ID:    return new MovieConsolidatedDTO(activity).GetInfoForBackup();
                case ActivityCategory.PET_ID:      return new PetConsolidatedDTO(activity).GetInfoForBackup();
                case ActivityCategory.MEET_ID:     return new MeetConsolidatedDTO(activity).GetInfoForBackup();
                case ActivityCategory.PLAY_ID:     return new PlayConsolidatedDTO(activity).GetInfoForBackup();
                case ActivityCategory.PURCHASE_ID: return new PurchaseConsolidatedDTO(activity).GetInfoForBackup();
                case ActivityCategory.SHOW_ID:     return new ShowConsolidatedDTO(activity).GetInfoForBackup();
                case ActivityCategory.TRAVEL_ID:   return new ConsolidatedTravelDTO(activity).GetInfoForBackup();
                case ActivityCategory.WORK_ID:     return new ConsolidatedWorkDTO(activity).GetInfoForBackup();
            }
            return "";
        }
    }
}
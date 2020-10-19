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

        public static Activity Create(ConsolidatedActivityDTO consolidated, UnitOfWork unitOfWork)
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

        public static ActivityBlock GetActivityBlockByName(string blockName, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(blockName)) {
                return null;
            }
            return unitOfWork.ActivityRepo.GetActivityBlockByName(blockName);
        }

        public static ActivityBlock CreateActivityBlock(string blockName, UnitOfWork unitOfWork)
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

        public static void SaveFromRawSegments(Activity activity, string[] rawSegments, UnitOfWork unitOfWork)
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

        public static void BackupToFile(string fileDir, int categoryId)
        {
            List<Activity> activities;
            ActivityCategory category;
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                activities = unitOfWork.ActivityRepo.GetAllInclusiveFromCategory(categoryId);
                category = unitOfWork.ActivityRepo.GetCategoryById(categoryId);
            }

            var filePath = fileDir + category.Name + ".txt";

            if (activities.Count == 0) {
                return;
            }

            using (var file = new StreamWriter(filePath)) {
                foreach (var activity in activities) {
                    string activityString = GetInfoForBackup(activity);
                    if (!string.IsNullOrWhiteSpace(activityString)) {
                        file.WriteLine(activityString);
                    }
                }
            }
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

        public static void RestoreFromFile(string fileDir, int categoryId)
        {
            ActivityCategory category;
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                category = unitOfWork.ActivityRepo.GetCategoryById(categoryId);

                unitOfWork.ActivityRepo.DeleteAllFromCategory(categoryId);
                unitOfWork.Complete();

                using (var reader = new StreamReader(fileDir + category.Name + ".txt")) {
                    string line = "";
                    while ((line = reader.ReadLine()) != null) {
                        if (string.IsNullOrWhiteSpace(line)) {
                            continue;
                        }

                        var backupSegments = Regex.Split(line, "\t");

                        SaveActivityFromBackupSegments(backupSegments, category, unitOfWork);
                    }
                }

                unitOfWork.Complete();
            }
        }

        private static void SaveActivityFromBackupSegments(string[] backupSegments, ActivityCategory category, UnitOfWork unitOfWork)
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
    }
}
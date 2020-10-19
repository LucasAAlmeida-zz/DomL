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
    public class DoomService
    {
        public static void SaveFromRawSegments(string[] rawSegments, Activity activity, UnitOfWork unitOfWork)
        {
            var consolidated = new ConsolidatedDoomDTO(rawSegments, activity);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        public static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            var consolidated = new ConsolidatedDoomDTO(backupSegments);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        private static void SaveFromConsolidated(ConsolidatedDoomDTO consolidated, UnitOfWork unitOfWork)
        {
            var activity = ActivityService.Create(consolidated, unitOfWork);
            CreateDoomActivity(activity, consolidated.Description, unitOfWork);
        }

        private static void CreateDoomActivity(Activity activity, string description, UnitOfWork unitOfWork)
        {
            var doomActivity = new DoomActivity() {
                Activity = activity,
                Description = description
            };

            activity.DoomActivity = doomActivity;
            ActivityService.PairUpWithStartingActivity(activity, unitOfWork);

            unitOfWork.DoomRepo.CreateDoomActivity(doomActivity);
        }

        public static IEnumerable<Activity> GetStartingActivities(IQueryable<Activity> previousStartingActivities, Activity activity)
        {
            var description = activity.DoomActivity.Description;
            return previousStartingActivities.Where(u =>
                u.CategoryId == ActivityCategory.DOOM_ID
                && u.DoomActivity.Description == description
            );
        }

        public static void RestoreFromFile(string fileDir)
        {
            using (var reader = new StreamReader(fileDir + "Doom.txt")) {
                string line = "";
                while ((line = reader.ReadLine()) != null) {
                    if (string.IsNullOrWhiteSpace(line)) {
                        continue;
                    }

                    var segments = Regex.Split(line, "\t");

                    // Date; Auto Name; Description
                    var date = segments[0];
                    var description = segments[1];

                    var originalLine = "DOOM; " + description;

                    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                        var statusSingle = unitOfWork.ActivityRepo.GetStatusById(ActivityStatus.SINGLE);
                        var category = unitOfWork.ActivityRepo.GetCategoryById(ActivityCategory.DOOM_ID);

                        var dateDT = DateTime.ParseExact(date, "dd/MM/yy", null);
                        var activity = ActivityService.Create(dateDT, 0, statusSingle, category, null, originalLine, unitOfWork);

                        CreateDoomActivity(activity, description, unitOfWork);

                        unitOfWork.Complete();
                    }
                }
            }
        }
    }
}

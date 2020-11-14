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
            var consolidated = new DoomConsolidatedDTO(rawSegments, activity);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        public static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            var consolidated = new DoomConsolidatedDTO(backupSegments);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        private static void SaveFromConsolidated(DoomConsolidatedDTO consolidated, UnitOfWork unitOfWork)
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
    }
}

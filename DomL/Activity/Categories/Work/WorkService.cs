using DomL.Business.DTOs;
using DomL.Business.Entities;
using DomL.Business.Utils;
using DomL.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DomL.Business.Services
{
    public class WorkService
    {
        public static void SaveFromRawSegments(string[] rawSegments, Activity activity, UnitOfWork unitOfWork)
        {
            var consolidated = new WorkConsolidatedDTO(rawSegments, activity);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        public static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            var consolidated = new WorkConsolidatedDTO(backupSegments);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        private static void SaveFromConsolidated(WorkConsolidatedDTO consolidated, UnitOfWork unitOfWork)
        {
            var activity = ActivityService.Create(consolidated, unitOfWork);
            CreateWorkActivity(activity, consolidated.Work, consolidated.Description, unitOfWork);
        }

        private static void CreateWorkActivity(Activity activity, string work, string description, UnitOfWork unitOfWork)
        {
            var workActivity = new WorkActivity() {
                Activity = activity,
                Work = Util.GetStringOrNull(work),
                Description = Util.GetStringOrNull(description)
            };

            activity.WorkActivity = workActivity;
            ActivityService.PairUpWithStartingActivity(activity, unitOfWork);

            unitOfWork.WorkRepo.CreateWorkActivity(workActivity);
        }

        public static IEnumerable<Activity> GetStartingActivities(IQueryable<Activity> previousStartingActivities, Activity activity)
        {
            var work = activity.WorkActivity.Work;
            return previousStartingActivities.Where(u =>
                u.CategoryId == Category.WORK_ID
                && u.WorkActivity.Work == work
            );
        }
    }
}

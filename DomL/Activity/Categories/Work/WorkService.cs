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
    public class WorkService
    {
        public static void SaveFromRawSegments(string[] rawSegments, Activity activity, UnitOfWork unitOfWork)
        {
            var consolidated = new ConsolidatedWorkDTO(rawSegments, activity);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        public static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            var consolidated = new ConsolidatedWorkDTO(backupSegments);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        private static void SaveFromConsolidated(ConsolidatedWorkDTO consolidated, UnitOfWork unitOfWork)
        {
            var work = CompanyService.GetOrCreateByName(consolidated.WorkName, unitOfWork);

            var activity = ActivityService.Create(consolidated, unitOfWork);
            CreateWorkActivity(activity, work, consolidated.Description, unitOfWork);
        }

        private static void CreateWorkActivity(Activity activity, Company work, string description, UnitOfWork unitOfWork)
        {
            var workActivity = new WorkActivity() {
                Activity = activity,
                Work = work,
                Description = description
            };

            activity.WorkActivity = workActivity;
            ActivityService.PairUpWithStartingActivity(activity, unitOfWork);

            unitOfWork.WorkRepo.CreateWorkActivity(workActivity);
        }

        public static IEnumerable<Activity> GetStartingActivities(IQueryable<Activity> previousStartingActivities, Activity activity)
        {
            var work = activity.WorkActivity.Work;
            return previousStartingActivities.Where(u =>
                u.CategoryId == ActivityCategory.WORK_ID
                && u.WorkActivity.Work.Name == work.Name
            );
        }
    }
}

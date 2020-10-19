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
        public static void SaveFromRawSegments(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            // WORK; Work Name; Description
            var workName = segments[1];
            var description = segments[2];

            Company work = CompanyService.GetOrCreateByName(workName, unitOfWork);

            CreateWorkActivity(activity, work, description, unitOfWork);
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

        public static void RestoreFromFile(string fileDir)
        {
            using (var reader = new StreamReader(fileDir + "Work.txt")) {
                string line = "";
                while ((line = reader.ReadLine()) != null) {
                    if (string.IsNullOrWhiteSpace(line)) {
                        continue;
                    }

                    var segments = Regex.Split(line, "\t");

                    // Date; Work Name; Description
                    var date = segments[0];
                    var workName = segments[1];
                    var description = segments[2];

                    var originalLine = "WORK; " + workName + "; " + description;

                    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                        var work = CompanyService.GetOrCreateByName(workName, unitOfWork);
                        var statusSingle = unitOfWork.ActivityRepo.GetStatusById(ActivityStatus.SINGLE);
                        var category = unitOfWork.ActivityRepo.GetCategoryById(ActivityCategory.WORK_ID);

                        var dateDT = DateTime.ParseExact(date, "dd/MM/yy", null);
                        var activity = ActivityService.Create(dateDT, 0, statusSingle, category, null, originalLine, unitOfWork);

                        CreateWorkActivity(activity, work, description, unitOfWork);

                        unitOfWork.Complete();
                    }
                }
            }
        }

        internal static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            throw new NotImplementedException();
        }
    }
}

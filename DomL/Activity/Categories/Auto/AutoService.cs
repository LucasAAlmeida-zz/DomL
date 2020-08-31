using DomL.Business.Entities;
using DomL.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DomL.Business.Services
{
    public class AutoService
    {
        public static void SaveFromRawSegments(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            // AUTO; Auto Name; Description
            var autoName = segments[1];
            var description = segments[2];

            var auto = TransportService.GetOrCreateByName(autoName, unitOfWork);

            CreateAutoActivity(activity, auto, description, unitOfWork);
        }

        private static void CreateAutoActivity(Activity activity, Transport auto, string description, UnitOfWork unitOfWork)
        {
            var autoActivity = new AutoActivity() {
                Activity = activity,
                Auto = auto,
                Description = description
            };

            activity.AutoActivity = autoActivity;
            ActivityService.PairUpWithStartingActivity(activity, unitOfWork);

            unitOfWork.AutoRepo.CreateAutoActivity(autoActivity);
        }

        public static IEnumerable<Activity> GetStartingActivities(IQueryable<Activity> previousStartingActivities, Activity activity)
        {
            var auto = activity.AutoActivity.Auto;
            return previousStartingActivities.Where(u =>
                u.CategoryId == ActivityCategory.AUTO_ID
                && u.AutoActivity.Auto.Name == auto.Name
            );
        }

        public static void RestoreFromFile(string fileDir)
        {
            using (var reader = new StreamReader(fileDir + "Auto.txt")) {
                string line = "";
                while ((line = reader.ReadLine()) != null) {
                    if (string.IsNullOrWhiteSpace(line)) {
                        continue;
                    }

                    var segments = Regex.Split(line, "\t");

                    // Date; Auto Name; Description
                    var date = segments[0];
                    var autoName = segments[1];
                    var description = segments[2];

                    var originalLine = "AUTO; " + autoName + "; " + description;

                    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                        var auto = TransportService.GetOrCreateByName(autoName, unitOfWork);
                        var statusSingle = unitOfWork.ActivityRepo.GetStatusById(ActivityStatus.SINGLE);
                        var category = unitOfWork.ActivityRepo.GetCategoryById(ActivityCategory.AUTO_ID);

                        var dateDT = DateTime.ParseExact(date, "dd/MM/yy", null);
                        var activity = ActivityService.Create(dateDT, 0, statusSingle, category, null, originalLine, unitOfWork);

                        CreateAutoActivity(activity, auto, description, unitOfWork);

                        unitOfWork.Complete();
                    }
                }
            }
        }
    }
}

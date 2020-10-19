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
    public class AutoService
    {
        public static void SaveFromRawSegments(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            // AUTO; Auto Name; Description
            var autoName = segments[1];
            var description = segments[2];

            var auto = TransportService.CreateOrGetByName(autoName, unitOfWork);

            CreateAutoActivity(activity, auto, description, unitOfWork);
        }

        public static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            var consolidated = new ConsolidatedAutoDTO(backupSegments);

            var auto = TransportService.CreateOrGetByName(consolidated.AutoName, unitOfWork);
            
            var activity = ActivityService.Create(consolidated, unitOfWork);
            CreateAutoActivity(activity, auto, consolidated.Description, unitOfWork);
        }

        public static void CreateAutoActivity(Activity activity, Transport auto, string description, UnitOfWork unitOfWork)
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
    }
}

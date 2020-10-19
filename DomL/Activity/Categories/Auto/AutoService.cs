using DomL.Business.DTOs;
using DomL.Business.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DomL.Business.Services
{
    public class AutoService
    {
        public static void SaveFromRawSegments(string[] rawSegments, Activity activity, UnitOfWork unitOfWork)
        {
            var consolidated = new ConsolidatedAutoDTO(rawSegments, activity);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        public static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            var consolidated = new ConsolidatedAutoDTO(backupSegments);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        private static void SaveFromConsolidated(ConsolidatedAutoDTO consolidated, UnitOfWork unitOfWork)
        {
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

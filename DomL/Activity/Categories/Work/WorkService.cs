using DomL.Business.Entities;
using System.Collections.Generic;
using System.Linq;

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
    }
}

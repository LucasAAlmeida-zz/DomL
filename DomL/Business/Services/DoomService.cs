using DomL.Business.DTOs;
using DomL.Business.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DomL.Business.Services
{
    public class DoomService
    {
        public static void SaveFromRawSegments(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            // DOOM; Description
            var description = segments[1];

            CreateDoomActivity(activity, description, unitOfWork);
        }

        private static void CreateDoomActivity(Activity activity, string description, UnitOfWork unitOfWork)
        {
            var doomActivity = new DoomActivity() {
                Activity = activity,
                Description = description
            };

            activity.DoomActivity = doomActivity;
            activity.PairUpActivity(unitOfWork);

            unitOfWork.DoomRepo.CreateDoomActivity(doomActivity);
        }

        public static IEnumerable<Activity> GetStartingActivities(IQueryable<Activity> previousStartingActivities, Activity activity)
        {
            var description = activity.DoomActivity.Description;
            return previousStartingActivities.Where(u =>
                u.CategoryId == ActivityCategory.DOOM
                && u.DoomActivity.Description == description
            );
        }
    }
}

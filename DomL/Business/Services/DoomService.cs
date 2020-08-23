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
            activity.PairActivity(unitOfWork);

            unitOfWork.DoomRepo.CreateDoomActivity(doomActivity);
        }

        public static string GetString(Activity activity, int kindOfString)
        {
            var consolidatedInfo = new ConsolidatedDoomActivityDTO(activity);
            switch (kindOfString) {
                case 0:     return consolidatedInfo.GetInfoForMonthRecap();
                case 1:     return consolidatedInfo.GetInfoForYearRecap();
                case 2:     return consolidatedInfo.GetInfoForYearRecap();
                default:    return "";
            }
        }

        public static IEnumerable<Activity> GetStartingActivity(List<Activity> previousStartingActivities, Activity activity)
        {
            var description = activity.DoomActivity.Description;
            return previousStartingActivities.Where(u =>
                u.CategoryId == ActivityCategory.DOOM
                && IsSameDoom(u.DoomActivity.Description, description)
            );
        }

        private static bool IsSameDoom(string description1, string description2)
        {
            return description1 == description2;
        }
    }
}

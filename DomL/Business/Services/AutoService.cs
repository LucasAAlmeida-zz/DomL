using DomL.Business.DTOs;
using DomL.Business.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DomL.Business.Services
{
    public class AutoService
    {
        public static void SaveFromRawSegments(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            // AUTO; Auto Name; Description
            var autoName = segments[1];
            var description = segments[2];

            Auto auto = GetOrCreateAuto(autoName, unitOfWork);
            CreateAutoActivity(activity, auto, description, unitOfWork);
        }

        private static void CreateAutoActivity(Activity activity, Auto auto, string description, UnitOfWork unitOfWork)
        {
            var autoActivity = new AutoActivity() {
                Activity = activity,
                Auto = auto,
                Description = description
            };

            activity.AutoActivity = autoActivity;
            activity.PairActivity(unitOfWork);

            unitOfWork.AutoRepo.CreateAutoActivity(autoActivity);
        }

        private static Auto GetOrCreateAuto(string autoName, UnitOfWork unitOfWork)
        {
            var auto = unitOfWork.AutoRepo.GetAutoByName(autoName);
            
            if (auto == null) {
                auto = new Auto() {
                    Name = autoName
                };
                unitOfWork.AutoRepo.Add(auto);
            }

            return auto;
        }

        public static string GetString(Activity activity, int kindOfString)
        {
            var consolidatedInfo = new ConsolidatedAutoActivityDTO(activity);
            switch (kindOfString) {
                case 0:     return consolidatedInfo.GetInfoForMonthRecap();
                case 1:     return consolidatedInfo.GetInfoForYearRecap();
                case 2:     return consolidatedInfo.GetInfoForBackup();
                default:    return "";
            }
        }

        public static IEnumerable<Activity> GetStartingActivity(List<Activity> previousStartingActivities, Activity activity)
        {
            var auto = activity.AutoActivity.Auto;
            return previousStartingActivities.Where(u =>
                u.CategoryId == ActivityCategory.AUTO
                && IsSameAuto(u.AutoActivity.Auto, auto)
            );
        }

        private static bool IsSameAuto(Auto auto1, Auto auto2)
        {
            return auto1.Name == auto2.Name;
        }
    }
}

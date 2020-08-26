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

            Auto auto = GetOrCreateAutoByName(autoName, unitOfWork);

            CreateAutoActivity(activity, auto, description, unitOfWork);
        }

        private static Auto GetOrCreateAutoByName(string autoName, UnitOfWork unitOfWork)
        {
            var auto = unitOfWork.AutoRepo.GetAutoByName(autoName);

            if (auto == null) {
                auto = new Auto() {
                    Name = autoName
                };
                unitOfWork.AutoRepo.CreateAuto(auto);
            }

            return auto;
        }

        private static void CreateAutoActivity(Activity activity, Auto auto, string description, UnitOfWork unitOfWork)
        {
            var autoActivity = new AutoActivity() {
                Activity = activity,
                Auto = auto,
                Description = description
            };

            activity.AutoActivity = autoActivity;
            activity.PairUpActivity(unitOfWork);

            unitOfWork.AutoRepo.CreateAutoActivity(autoActivity);
        }

        public static IEnumerable<Activity> GetStartingActivity(IQueryable<Activity> previousStartingActivities, Activity activity)
        {
            var auto = activity.AutoActivity.Auto;
            return previousStartingActivities.Where(u =>
                u.CategoryId == ActivityCategory.AUTO
                && u.AutoActivity.Auto.Name == auto.Name
            );
        }
    }
}

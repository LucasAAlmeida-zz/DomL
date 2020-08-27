using DomL.Business.DTOs;
using DomL.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DomL.Business.Services
{
    public class HealthService
    {
        public static void SaveFromRawSegments(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            // HEALTH; (Medical Specialty Name); Description
            string specialtyName = null;
            string description;
            if (segments.Length == 2) {
                description = segments[1];
            } else {
                specialtyName = segments[1];
                description = segments[2];
            }

            Company specialty = CompanyService.GetOrCreateByName(specialtyName, unitOfWork);

            CreateHealthActivity(activity, specialty, description, unitOfWork);
        }

        private static void CreateHealthActivity(Activity activity, Company specialty, string description, UnitOfWork unitOfWork)
        {
            var healthActivity = new HealthActivity() {
                Activity = activity,
                Specialty = specialty,
                Description = description
            };

            activity.HealthActivity = healthActivity;
            activity.PairUpActivity(unitOfWork);

            unitOfWork.HealthRepo.CreateHealthActivity(healthActivity);
        }

        public static IEnumerable<Activity> GetStartingActivities(IQueryable<Activity> previousStartingActivities, Activity activity)
        {
            var healthActivity = activity.HealthActivity;
            return previousStartingActivities.Where(u =>
                u.CategoryId == ActivityCategory.HEALTH
                && u.HealthActivity.Description == healthActivity.Description
            );
        }
    }
}

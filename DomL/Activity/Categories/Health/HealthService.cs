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
    public class HealthService
    {
        public static void SaveFromRawSegments(string[] rawSegments, Activity activity, UnitOfWork unitOfWork)
        {
            var consolidated = new ConsolidatedHealthDTO(rawSegments, activity);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        public static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            var consolidated = new ConsolidatedHealthDTO(backupSegments);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        private static void SaveFromConsolidated(ConsolidatedHealthDTO consolidated, UnitOfWork unitOfWork)
        {
            Company specialty = CompanyService.GetOrCreateByName(consolidated.MedicalSpecialtyName, unitOfWork);

            var activity = ActivityService.Create(consolidated, unitOfWork);
            CreateHealthActivity(activity, specialty, consolidated.Description, unitOfWork);
        }

        private static void CreateHealthActivity(Activity activity, Company specialty, string description, UnitOfWork unitOfWork)
        {
            var healthActivity = new HealthActivity() {
                Activity = activity,
                Specialty = specialty,
                Description = description
            };

            activity.HealthActivity = healthActivity;
            ActivityService.PairUpWithStartingActivity(activity, unitOfWork);

            unitOfWork.HealthRepo.CreateHealthActivity(healthActivity);
        }

        public static IEnumerable<Activity> GetStartingActivities(IQueryable<Activity> previousStartingActivities, Activity activity)
        {
            var healthActivity = activity.HealthActivity;
            return previousStartingActivities.Where(u =>
                u.CategoryId == ActivityCategory.HEALTH_ID
                && u.HealthActivity.Description == healthActivity.Description
            );
        }
    }
}

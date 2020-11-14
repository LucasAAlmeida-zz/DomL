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
    public class PetService
    {
        public static void SaveFromRawSegments(string[] rawSegments, Activity activity, UnitOfWork unitOfWork)
        {
            var consolidated = new PetConsolidatedDTO(rawSegments, activity);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        public static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            var consolidated = new PetConsolidatedDTO(backupSegments);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        private static void SaveFromConsolidated(PetConsolidatedDTO consolidated, UnitOfWork unitOfWork)
        {
            var activity = ActivityService.Create(consolidated, unitOfWork);
            CreatePetActivity(activity, consolidated.Pet, consolidated.Description, unitOfWork);
        }

        private static void CreatePetActivity(Activity activity, string pet, string description, UnitOfWork unitOfWork)
        {
            var petActivity = new PetActivity() {
                Activity = activity,
                Pet = pet,
                Description = description
            };

            activity.PetActivity = petActivity;
            ActivityService.PairUpWithStartingActivity(activity, unitOfWork);

            unitOfWork.PetRepo.CreatePetActivity(petActivity);
        }

        public static IEnumerable<Activity> GetStartingActivities(IQueryable<Activity> previousStartingActivities, Activity activity)
        {
            var pet = activity.PetActivity.Pet;
            return previousStartingActivities.Where(u =>
                u.CategoryId == ActivityCategory.AUTO_ID
                && u.PetActivity.Pet == pet
            );
        }
    }
}

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
        public static void SaveFromRawSegments(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            // AUTO; Pet Name; Description
            var petName = segments[1];
            var description = segments[2];

            Person pet = PersonService.GetOrCreateByName(petName, unitOfWork);

            CreatePetActivity(activity, pet, description, unitOfWork);
        }

        private static void CreatePetActivity(Activity activity, Person pet, string description, UnitOfWork unitOfWork)
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
                && u.PetActivity.Pet.Name == pet.Name
            );
        }

        public static void RestoreFromFile(string fileDir)
        {
            using (var reader = new StreamReader(fileDir + "Pet.txt")) {
                string line = "";
                while ((line = reader.ReadLine()) != null) {
                    if (string.IsNullOrWhiteSpace(line)) {
                        continue;
                    }

                    var segments = Regex.Split(line, "\t");

                    // Date; Pet Name; Description
                    var date = segments[0];
                    var petName = segments[1];
                    var description = segments[2];

                    var originalLine = "PET; " + petName + "; " + description;

                    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                        var pet = PersonService.GetOrCreateByName(petName, unitOfWork);
                        var statusSingle = unitOfWork.ActivityRepo.GetStatusById(ActivityStatus.SINGLE);
                        var category = unitOfWork.ActivityRepo.GetCategoryById(ActivityCategory.PET_ID);

                        var dateDT = DateTime.ParseExact(date, "dd/MM/yy", null);
                        var activity = ActivityService.Create(dateDT, 0, statusSingle, category, null, originalLine, unitOfWork);

                        CreatePetActivity(activity, pet, description, unitOfWork);

                        unitOfWork.Complete();
                    }
                }
            }
        }
    }
}

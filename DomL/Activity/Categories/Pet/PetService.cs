using DomL.Business.Entities;
using System.Collections.Generic;
using System.Linq;

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
    }
}

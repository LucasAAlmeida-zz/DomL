using DomL.Business.DTOs;
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

            Pet pet = GetOrCreatePetByName(petName, unitOfWork);

            CreatePetActivity(activity, pet, description, unitOfWork);
        }

        private static Pet GetOrCreatePetByName(string petName, UnitOfWork unitOfWork)
        {
            var pet = unitOfWork.PetRepo.GetPetByName(petName);

            if (pet == null) {
                pet = new Pet() {
                    Name = petName
                };
                unitOfWork.PetRepo.CreatePet(pet);
            }

            return pet;
        }

        private static void CreatePetActivity(Activity activity, Pet pet, string description, UnitOfWork unitOfWork)
        {
            var petActivity = new PetActivity() {
                Activity = activity,
                Pet = pet,
                Description = description
            };

            activity.PetActivity = petActivity;
            activity.PairUpActivity(unitOfWork);

            unitOfWork.PetRepo.CreatePetActivity(petActivity);
        }

        public static IEnumerable<Activity> GetStartingActivities(IQueryable<Activity> previousStartingActivities, Activity activity)
        {
            var pet = activity.PetActivity.Pet;
            return previousStartingActivities.Where(u =>
                u.CategoryId == ActivityCategory.AUTO
                && u.PetActivity.Pet.Name == pet.Name
            );
        }
    }
}

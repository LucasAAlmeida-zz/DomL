using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class ConsolidatedPetDTO : ConsolidatedActivityDTO
    {
        public string PetName;
        public string Description;

        public ConsolidatedPetDTO(Activity activity) : base(activity)
        {
            var petActivity = activity.PetActivity;
            var pet = petActivity.Pet;

            PetName = pet.Name;
            Description = petActivity.Description;
        }

        public string GetInfoForYearRecap()
        {
            // Date Started; Date Finished;
            // Pet Name; Description
            return DatesStartAndFinish
                + "\t" + PetName + "\t" + Description;
        }
    }
}

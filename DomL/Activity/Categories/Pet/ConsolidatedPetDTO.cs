using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class ConsolidatedPetDTO : ActivityConsolidatedDTO
    {
        public string PetName;
        public string Description;

        public ConsolidatedPetDTO(Activity activity) : base(activity)
        {
            CategoryName = "PET";

            var petActivity = activity.PetActivity;
            var pet = petActivity.Pet;

            PetName = pet.Name;
            Description = petActivity.Description;
        }

        public ConsolidatedPetDTO(string[] rawSegments, Activity activity) : base(activity)
        {
            CategoryName = "PET";

            PetName = rawSegments[1];
            Description = rawSegments[2];
        }

        public ConsolidatedPetDTO(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "PET";

            PetName = backupSegments[4];
            Description = backupSegments[5];

            OriginalLine = GetInfoForOriginalLine()
                + GetPetActivityInfo().Replace("\t", "; ");
        }

        public new string GetInfoForYearRecap()
        {
            return base.GetInfoForYearRecap()
                + "\t" + GetPetActivityInfo();
        }

        public new string GetInfoForBackup()
        {
            return base.GetInfoForBackup()
                + "\t" + GetPetActivityInfo();
        }

        public string GetPetActivityInfo()
        {
            return PetName + "\t" + Description;
        }
    }
}

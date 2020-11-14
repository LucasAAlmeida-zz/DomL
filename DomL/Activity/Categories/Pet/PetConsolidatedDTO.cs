using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class PetConsolidatedDTO : ActivityConsolidatedDTO
    {
        public string Pet;
        public string Description;

        public PetConsolidatedDTO(Activity activity) : base(activity)
        {
            CategoryName = "PET";

            var petActivity = activity.PetActivity;

            Pet = petActivity.Pet;
            Description = petActivity.Description;
        }

        public PetConsolidatedDTO(string[] rawSegments, Activity activity) : base(activity)
        {
            CategoryName = "PET";

            Pet = rawSegments[1];
            Description = rawSegments[2];
        }

        public PetConsolidatedDTO(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "PET";

            Pet = backupSegments[4];
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
            return Pet + "\t" + Description;
        }
    }
}

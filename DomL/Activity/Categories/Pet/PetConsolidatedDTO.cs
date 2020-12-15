using DomL.Business.Entities;
using DomL.Business.Utils;

namespace DomL.Business.DTOs
{
    public class PetConsolidatedDTO : ActivityConsolidatedDTO
    {
        public string Pet;
        public string Description;

        public PetConsolidatedDTO(Activity activity) : base(activity)
        {
            var petActivity = activity.PetActivity;

            Pet = Util.GetStringOrDash(petActivity.Pet);
            Description = Util.GetStringOrDash(petActivity.Description);

            FillCommonInfo();
        }

        public PetConsolidatedDTO(string[] rawSegments, Activity activity) : base(activity)
        {
            Pet = Util.GetStringOrDash(rawSegments[1]);
            Description = Util.GetStringOrDash(rawSegments[2]);

            FillCommonInfo();
        }

        public PetConsolidatedDTO(string[] backupSegments) : base(backupSegments)
        {
            Pet = backupSegments[4];
            Description = backupSegments[5];

            FillCommonInfo();
        }

        private void FillCommonInfo()
        {
            CategoryName = "PET";
            ConsolidatedLine = GetInfoForConsolidatedLine() + "; "
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

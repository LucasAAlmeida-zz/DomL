using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class ConsolidatedHealthDTO : ActivityConsolidatedDTO
    {
        public string MedicalSpecialtyName;
        public string Description;

        public ConsolidatedHealthDTO(Activity activity) : base(activity)
        {
            CategoryName = "HEALTH";

            var healthActivity = activity.HealthActivity;
            var specialty = healthActivity.Specialty;

            MedicalSpecialtyName = (specialty != null) ? specialty.Name : "-";
            Description = healthActivity.Description;
        }

        public ConsolidatedHealthDTO(string[] rawSegments, Activity activity) : base(activity)
        {
            CategoryName = "HEALTH";

            MedicalSpecialtyName = null;
            Description = rawSegments[1];
            if (rawSegments.Length > 2) {
                MedicalSpecialtyName = rawSegments[1];
                Description = rawSegments[2];
            }
        }

        public ConsolidatedHealthDTO(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "HEALTH";

            MedicalSpecialtyName = backupSegments[4];
            Description = backupSegments[5];

            OriginalLine = GetInfoForOriginalLine()
                + GetHealthActivityInfo().Replace("\t", "; ");
        }

        public new string GetInfoForYearRecap()
        {
            return base.GetInfoForYearRecap()
                + "\t" + GetHealthActivityInfo();
        }

        public new string GetInfoForBackup()
        {
            return base.GetInfoForBackup()
                + "\t" + GetHealthActivityInfo();
        }

        public string GetHealthActivityInfo()
        {
            return MedicalSpecialtyName + "\t" + Description;
        }
    }
}

using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class HealthConsolidatedDTO : ActivityConsolidatedDTO
    {
        public string Specialty;
        public string Description;

        public HealthConsolidatedDTO(Activity activity) : base(activity)
        {
            CategoryName = "HEALTH";

            var healthActivity = activity.HealthActivity;

            Specialty = healthActivity.Specialty ??  "-";
            Description = healthActivity.Description;
        }

        public HealthConsolidatedDTO(string[] rawSegments, Activity activity) : base(activity)
        {
            CategoryName = "HEALTH";

            Specialty = null;
            Description = rawSegments[1];
            if (rawSegments.Length > 2) {
                Specialty = rawSegments[1];
                Description = rawSegments[2];
            }
        }

        public HealthConsolidatedDTO(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "HEALTH";

            Specialty = backupSegments[4];
            Description = backupSegments[5];

            OriginalLine = GetInfoForOriginalLine() + "; "
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
            return Specialty + "\t" + Description;
        }
    }
}

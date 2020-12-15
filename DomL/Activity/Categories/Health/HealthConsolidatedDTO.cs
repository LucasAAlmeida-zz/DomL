using DomL.Business.Entities;
using DomL.Business.Utils;

namespace DomL.Business.DTOs
{
    public class HealthConsolidatedDTO : ActivityConsolidatedDTO
    {
        public string Specialty;
        public string Description;

        public HealthConsolidatedDTO(Activity activity) : base(activity)
        {
            var healthActivity = activity.HealthActivity;

            Specialty = Util.GetStringOrDash(healthActivity.Specialty);
            Description = Util.GetStringOrDash(healthActivity.Description);

            FillCommonInfo();
        }

        public HealthConsolidatedDTO(string[] rawSegments, Activity activity) : base(activity)
        {
            Specialty = Util.GetStringOrDash(rawSegments.Length > 2 ? rawSegments[1] : "-");
            Description = Util.GetStringOrDash(rawSegments.Length > 2 ? rawSegments[2] : rawSegments[1]);

            FillCommonInfo();
        }

        public HealthConsolidatedDTO(string[] backupSegments) : base(backupSegments)
        {
            Specialty = backupSegments[4];
            Description = backupSegments[5];

            FillCommonInfo();
        }

        private void FillCommonInfo()
        {
            CategoryName = "HEALTH";
            ConsolidatedLine = GetInfoForConsolidatedLine() + "; "
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

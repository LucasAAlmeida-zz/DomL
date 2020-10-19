using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class ConsolidatedAutoDTO : ConsolidatedActivityDTO
    {
        public string AutoName;
        public string Description;

        public ConsolidatedAutoDTO(Activity activity) : base(activity)
        {
            CategoryName = "AUTO";

            var autoActivity = activity.AutoActivity;
            var auto = autoActivity.Auto;

            AutoName = auto.Name;
            Description = autoActivity.Description;
        }

        public ConsolidatedAutoDTO(string[] rawSegments, Activity activity) : base(activity)
        {
            CategoryName = "AUTO";

            AutoName = rawSegments[1];
            Description = rawSegments[2];
        }

        public ConsolidatedAutoDTO(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "AUTO";

            AutoName = backupSegments[4];
            Description = backupSegments[5];

            OriginalLine = GetInfoForOriginalLine()
                + GetAutoActivityInfo().Replace("\t", "; ");
        }

        public new string GetInfoForYearRecap()
        {
            return base.GetInfoForYearRecap()
                + "\t" + GetAutoActivityInfo();
        }

        public new string GetInfoForBackup()
        {
            return base.GetInfoForBackup() 
                + "\t" + GetAutoActivityInfo();
        }

        private string GetAutoActivityInfo()
        {
            return AutoName + "\t" + Description;
        }
    }
}

using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class AutoConsolidatedDTO : ActivityConsolidatedDTO
    {
        public string Auto;
        public string Description;

        public AutoConsolidatedDTO(Activity activity) : base(activity)
        {
            CategoryName = "AUTO";

            var autoActivity = activity.AutoActivity;

            Auto = autoActivity.Auto;
            Description = autoActivity.Description;
        }

        public AutoConsolidatedDTO(string[] rawSegments, Activity activity) : base(activity)
        {
            CategoryName = "AUTO";

            Auto = rawSegments[1];
            Description = rawSegments[2];
        }

        public AutoConsolidatedDTO(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "AUTO";

            Auto = backupSegments[4];
            Description = backupSegments[5];

            OriginalLine = GetInfoForOriginalLine() + "; "
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
            return Auto + "\t" + Description;
        }
    }
}

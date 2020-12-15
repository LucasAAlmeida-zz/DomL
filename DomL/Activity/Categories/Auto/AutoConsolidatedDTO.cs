using DomL.Business.Entities;
using DomL.Business.Utils;

namespace DomL.Business.DTOs
{
    public class AutoConsolidatedDTO : ActivityConsolidatedDTO
    {
        public string Auto;
        public string Description;

        public AutoConsolidatedDTO(Activity activity) : base(activity)
        {
            var autoActivity = activity.AutoActivity;

            Auto = Util.GetStringOrDash(autoActivity.Auto);
            Description = Util.GetStringOrDash(autoActivity.Description);

            FillCommonInfo();
        }

        public AutoConsolidatedDTO(string[] rawSegments, Activity activity) : base(activity)
        {
            Auto = Util.GetStringOrDash(rawSegments[1]);
            Description = Util.GetStringOrDash(rawSegments[2]);

            FillCommonInfo();
        }

        public AutoConsolidatedDTO(string[] backupSegments) : base(backupSegments)
        {
            Auto = backupSegments[4];
            Description = backupSegments[5];

            FillCommonInfo();
        }

        private void FillCommonInfo()
        {
            CategoryName = "AUTO";
            ConsolidatedLine = GetInfoForConsolidatedLine()
                + "; " + GetAutoActivityInfo().Replace("\t", "; ");
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

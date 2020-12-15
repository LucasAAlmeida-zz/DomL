using DomL.Business.Entities;
using DomL.Business.Utils;

namespace DomL.Business.DTOs
{
    public class WorkConsolidatedDTO : ActivityConsolidatedDTO
    {
        public string Work;
        public string Description;

        public WorkConsolidatedDTO(Activity activity) : base(activity)
        {
            var workActivity = activity.WorkActivity;

            Work = Util.GetStringOrDash(workActivity.Work);
            Description = Util.GetStringOrDash(workActivity.Description);

            FillCommonInfo();
        }

        public WorkConsolidatedDTO(string[] rawSegments, Activity activity) : base(activity)
        {
            Work = Util.GetStringOrDash(rawSegments[1]);
            Description = Util.GetStringOrDash(rawSegments[2]);

            FillCommonInfo();
        }

        public WorkConsolidatedDTO(string[] backupSegments) : base(backupSegments)
        {
            Work = backupSegments[4];
            Description = backupSegments[5];

            FillCommonInfo();
        }

        private void FillCommonInfo()
        {
            CategoryName = "WORK";
            ConsolidatedLine = GetInfoForConsolidatedLine() + "; "
                + GetWorkActivityInfo().Replace("\t", "; ");
        }

        public new string GetInfoForYearRecap()
        {
            return base.GetInfoForYearRecap()
                + "\t" + GetWorkActivityInfo();
        }

        public new string GetInfoForBackup()
        {
            return base.GetInfoForBackup()
                + "\t" + GetWorkActivityInfo();
        }

        private string GetWorkActivityInfo()
        {
            return Work + "\t" + Description;
        }
    }
}

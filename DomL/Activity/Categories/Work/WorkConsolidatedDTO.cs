using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class WorkConsolidatedDTO : ActivityConsolidatedDTO
    {
        public string Work;
        public string Description;

        public WorkConsolidatedDTO(Activity activity) : base(activity)
        {
            CategoryName = "WORK";

            var workActivity = activity.WorkActivity;

            Work = workActivity.Work;
            Description = workActivity.Description;
        }

        public WorkConsolidatedDTO(string[] rawSegments, Activity activity) : base(activity)
        {
            CategoryName = "WORK";

            Work = rawSegments[1];
            Description = rawSegments[2];
        }

        public WorkConsolidatedDTO(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "WORK";

            Work = backupSegments[4];
            Description = backupSegments[5];

            OriginalLine = GetInfoForOriginalLine() + "; "
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

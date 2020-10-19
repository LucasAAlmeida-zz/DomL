using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class ConsolidatedWorkDTO : ConsolidatedActivityDTO
    {
        public string WorkName;
        public string Description;

        public ConsolidatedWorkDTO(Activity activity) : base(activity)
        {
            CategoryName = "WORK";

            var workActivity = activity.WorkActivity;
            var work = workActivity.Work;

            WorkName = work.Name;
            Description = workActivity.Description;
        }

        public ConsolidatedWorkDTO(string[] rawSegments, Activity activity) : base(activity)
        {
            CategoryName = "WORK";

            WorkName = rawSegments[1];
            Description = rawSegments[2];
        }

        public ConsolidatedWorkDTO(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "WORK";

            WorkName = backupSegments[4];
            Description = backupSegments[5];

            OriginalLine = GetInfoForOriginalLine()
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
            return WorkName + "\t" + Description;
        }
    }
}

using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class MeetConsolidatedDTO : ActivityConsolidatedDTO
    {
        public string Person;
        public string Origin;
        public string Description;

        public MeetConsolidatedDTO(Activity activity) : base(activity)
        {
            var meetActivity = activity.MeetActivity;

            Person = meetActivity.Person;
            Origin = meetActivity.Origin;
            Description = meetActivity.Description;
        }

        public MeetConsolidatedDTO(string[] rawSegments, Activity activity) : base(activity)
        {
            CategoryName = "MEET";

            Person = rawSegments[1];
            Origin = rawSegments[2];
            Description = rawSegments[3];
        }

        public MeetConsolidatedDTO(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "MEET";

            Person = backupSegments[4];
            Origin = backupSegments[5];
            Description = backupSegments[6];

            OriginalLine = GetInfoForOriginalLine()
                + GetMeetActivityInfo().Replace("\t", "; ");
        }

        public new string GetInfoForYearRecap()
        {
            return base.GetInfoForYearRecap()
                + "\t" + GetMeetActivityInfo();
        }

        public new string GetInfoForBackup()
        {
            return base.GetInfoForBackup()
                + "\t" + GetMeetActivityInfo();
        }

        public string GetMeetActivityInfo()
        {
            return Person + "\t" + Origin + "\t" + Description;
        }
    }
}

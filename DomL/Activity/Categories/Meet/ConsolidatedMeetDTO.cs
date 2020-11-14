using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class ConsolidatedMeetDTO : ActivityConsolidatedDTO
    {
        public string PersonName;
        public string Origin;
        public string Description;

        public ConsolidatedMeetDTO(Activity activity) : base(activity)
        {
            var meetActivity = activity.MeetActivity;
            var person = meetActivity.Person;

            PersonName = person.Name;
            Origin = meetActivity.Origin;
            Description = meetActivity.Description;
        }

        public ConsolidatedMeetDTO(string[] rawSegments, Activity activity) : base(activity)
        {
            CategoryName = "MEET";

            PersonName = rawSegments[1];
            Origin = rawSegments[2];
            Description = rawSegments[3];
        }

        public ConsolidatedMeetDTO(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "MEET";

            PersonName = backupSegments[4];
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
            return PersonName + "\t" + Origin + "\t" + Description;
        }
    }
}

using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class ConsolidatedMeetActivityDTO : ConsolidatedActivityDTO
    {
        public string PersonName;
        public string Origin;
        public string Description;

        public ConsolidatedMeetActivityDTO(Activity activity) : base(activity)
        {
            var meetActivity = activity.MeetActivity;
            var person = meetActivity.Person;

            PersonName = person.Name;
            Origin = meetActivity.Origin;
            Description = meetActivity.Description;
        }

        public string GetInfoForYearRecap()
        {
            // Date Started; Date Finished;
            // Person Name; Origin; Description
            return DatesStartAndFinish
                + "\t" + PersonName + "\t" + Origin + "\t" + Description;
        }
    }
}

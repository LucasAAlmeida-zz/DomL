using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class ConsolidatedPlayActivityDTO : ConsolidatedActivityDTO
    {
        public string PersonName;
        public string Description;

        public ConsolidatedPlayActivityDTO(Activity activity) : base(activity)
        {
            var playActivity = activity.PlayActivity;
            var person = playActivity.Person;

            PersonName = person.Name;
            Description = playActivity.Description;
        }

        public string GetInfoForYearRecap()
        {
            // Date Started; Date Finished;
            // Person Name; Description
            return DatesStartAndFinish
                + "\t" + PersonName + "\t" + Description;
        }
    }
}

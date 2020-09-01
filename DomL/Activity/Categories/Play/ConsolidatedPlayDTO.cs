using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class ConsolidatedPlayDTO : ConsolidatedActivityDTO
    {
        public string Who;
        public string Description;

        public ConsolidatedPlayDTO(Activity activity) : base(activity)
        {
            var playActivity = activity.PlayActivity;

            Who = playActivity.Who;
            Description = playActivity.Description;
        }

        public string GetInfoForYearRecap()
        {
            // Date Started; Date Finished;
            // Person Name; Description
            return DatesStartAndFinish
                + "\t" + Who + "\t" + Description;
        }
    }
}

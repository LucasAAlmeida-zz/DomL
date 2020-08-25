using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class ConsolidatedDoomActivityDTO : ConsolidatedActivityDTO
    {
        public string Description;

        public ConsolidatedDoomActivityDTO(Activity activity) : base(activity)
        {
            var doomActivity = activity.DoomActivity;

            Description = doomActivity.Description;
        }

        public string GetInfoForYearRecap()
        {
            // Date Started; Date Finished;
            // Description
            return DatesStartAndFinish
                + "\t" + Description;
        }
    }
}

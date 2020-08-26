using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class ConsolidatedDoomDTO : ConsolidatedActivityDTO
    {
        public string Description;

        public ConsolidatedDoomDTO(Activity activity) : base(activity)
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

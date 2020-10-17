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
                + "\t" + GetDoomActivityInfo();
        }

        public new string GetInfoForBackup()
        {
            return base.GetInfoForBackup()
                + "\t" + GetDoomActivityInfo();
        }

        public string GetDoomActivityInfo()
        {
            return Description;
        }
    }
}

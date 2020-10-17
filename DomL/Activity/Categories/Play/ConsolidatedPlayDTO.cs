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
                + "\t" + GetPlayActivityInfo();
        }

        public new string GetInfoForBackup()
        {
            return base.GetInfoForBackup()
                + "\t" + GetPlayActivityInfo();
        }

        public string GetPlayActivityInfo()
        {
            return Who + "\t" + Description;
        }
    }
}

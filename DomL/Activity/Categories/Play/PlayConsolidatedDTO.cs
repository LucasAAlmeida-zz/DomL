using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class PlayConsolidatedDTO : ActivityConsolidatedDTO
    {
        public string Who;
        public string Description;

        public PlayConsolidatedDTO(Activity activity) : base(activity)
        {
            CategoryName = "PLAY";

            var playActivity = activity.PlayActivity;

            Who = playActivity.Who;
            Description = playActivity.Description;
        }

        public PlayConsolidatedDTO(string[] rawSegments, Activity activity) : base(activity)
        {
            CategoryName = "PLAY";

            Who = rawSegments[1];
            Description = rawSegments.Length > 2 ? rawSegments[2] : null;
        }

        public PlayConsolidatedDTO(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "PLAY";

            Who = backupSegments[4];
            Description = backupSegments[5];

            OriginalLine = GetInfoForOriginalLine() + "; "
                + GetPlayActivityInfo().Replace("\t", "; ");
        }

        public new string GetInfoForYearRecap()
        {
            return base.GetInfoForYearRecap()
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

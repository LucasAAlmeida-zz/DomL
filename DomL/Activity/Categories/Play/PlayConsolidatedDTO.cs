using DomL.Business.Entities;
using DomL.Business.Utils;

namespace DomL.Business.DTOs
{
    public class PlayConsolidatedDTO : ActivityConsolidatedDTO
    {
        public string Who;
        public string Description;

        public PlayConsolidatedDTO(Activity activity) : base(activity)
        {
            var playActivity = activity.PlayActivity;

            Who = Util.GetStringOrDash(playActivity.Who);
            Description = Util.GetStringOrDash(playActivity.Description);

            FillCommonInfo();
        }

        public PlayConsolidatedDTO(string[] rawSegments, Activity activity) : base(activity)
        {
            Who = Util.GetStringOrDash(rawSegments[1]);
            Description = Util.GetStringOrDash(rawSegments.Length > 2 ? rawSegments[2] : "-");

            FillCommonInfo();
        }

        public PlayConsolidatedDTO(string[] backupSegments) : base(backupSegments)
        {
            Who = backupSegments[4];
            Description = backupSegments[5];

            FillCommonInfo();
        }

        private void FillCommonInfo()
        {
            CategoryName = "PLAY";
            ConsolidatedLine = GetInfoForConsolidatedLine() + "; "
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

using DomL.Business.Entities;
using DomL.Business.Utils;

namespace DomL.Business.DTOs
{
    public class Consolidated : ActivityConsolidatedDTO
    {
        public string Transport;
        public string Origin;
        public string Destination;
        public string Description;

        public Consolidated(Activity activity) : base(activity)
        {
            var travelActivity = activity.TravelActivity;

            Transport = Util.GetStringOrDash(travelActivity.Transport);
            Origin = Util.GetStringOrDash(travelActivity.Origin);
            Destination = Util.GetStringOrDash(travelActivity.Destination);
            Description = Util.GetStringOrDash(travelActivity.Description);

            FillCommonInfo();
        }

        public Consolidated(string[] rawSegments, Activity activity) : base(activity)
        {
            Transport = Util.GetStringOrDash(rawSegments[1]);
            Origin = Util.GetStringOrDash(rawSegments[2]);
            Destination = Util.GetStringOrDash(rawSegments[3]);
            Description = Util.GetStringOrDash((rawSegments.Length > 4) ? rawSegments[4] : "-");

            FillCommonInfo();
        }

        public Consolidated(string[] backupSegments) : base(backupSegments)
        {
            Transport = backupSegments[4];
            Origin = backupSegments[5];
            Destination = backupSegments[6];
            Description = backupSegments[7];

            FillCommonInfo();
        }

        private void FillCommonInfo()
        {
            CategoryName = "TRAVEL";
            ConsolidatedLine = GetInfoForConsolidatedLine() + "; "
                + GetTravelActivityInfo().Replace("\t", "; ");
        }

        public new string GetInfoForYearRecap()
        {
            return base.GetInfoForYearRecap()
                + "\t" + GetTravelActivityInfo();
        }

        public new string GetInfoForBackup()
        {
            return base.GetInfoForBackup()
                + "\t" + GetTravelActivityInfo();
        }

        private string GetTravelActivityInfo()
        {
            return Transport + "\t" + Origin + "\t" + Destination + "\t" + Description;
        }
    }
}

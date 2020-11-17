using DomL.Business.Entities;

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
            CategoryName = "TRAVEL";

            var travelActivity = activity.TravelActivity;

            Transport = travelActivity.Transport;
            Origin = travelActivity.Origin;
            Destination = travelActivity.Destination;
            Description = travelActivity.Description;
        }

        public Consolidated(string[] rawSegments, Activity activity) : base(activity)
        {
            CategoryName = "TRAVEL";

            Transport = rawSegments[1];
            Origin = rawSegments[2];
            Destination = rawSegments[3];
            Description = (rawSegments.Length > 4) ? rawSegments[4] : null;
        }

        public Consolidated(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "TRAVEL";

            Transport = backupSegments[4];
            Origin = backupSegments[5];
            Destination = backupSegments[6];
            Description = backupSegments[7];

            OriginalLine = GetInfoForOriginalLine() + "; "
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

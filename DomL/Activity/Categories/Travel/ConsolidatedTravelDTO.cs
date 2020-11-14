using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class ConsolidatedTravelDTO : ActivityConsolidatedDTO
    {
        public string TransportName;
        public string OriginName;
        public string DestinationName;
        public string Description;

        public ConsolidatedTravelDTO(Activity activity) : base(activity)
        {
            CategoryName = "TRAVEL";

            var travelActivity = activity.TravelActivity;
            var transport = travelActivity.Transport;
            var origin = travelActivity.Origin;
            var destination = travelActivity.Destination;

            TransportName = transport.Name;
            OriginName = origin.Name;
            DestinationName = destination.Name;
            Description = travelActivity.Description;
        }

        public ConsolidatedTravelDTO(string[] rawSegments, Activity activity) : base(activity)
        {
            CategoryName = "TRAVEL";

            TransportName = rawSegments[1];
            OriginName = rawSegments[2];
            DestinationName = rawSegments[3];
            Description = (rawSegments.Length > 4) ? rawSegments[4] : null;
        }

        public ConsolidatedTravelDTO(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "TRAVEL";

            TransportName = backupSegments[4];
            OriginName = backupSegments[5];
            DestinationName = backupSegments[6];
            Description = backupSegments[7];

            OriginalLine = GetInfoForOriginalLine()
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
            return TransportName + "\t" + OriginName + "\t" + DestinationName + "\t" + Description;
        }
    }
}

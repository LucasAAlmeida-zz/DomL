using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class ConsolidatedTravelDTO : ConsolidatedActivityDTO
    {
        public string TransportName;
        public string OriginName;
        public string DestinationName;
        public string Description;

        public ConsolidatedTravelDTO(Activity activity) : base(activity)
        {
            var travelActivity = activity.TravelActivity;
            var transport = travelActivity.Transport;
            var origin = travelActivity.Origin;
            var destination = travelActivity.Destination;

            TransportName = transport.Name;
            OriginName = origin.Name;
            DestinationName = destination.Name;
            Description = travelActivity.Description;
        }

        public string GetInfoForYearRecap()
        {
            // Date Started; Date Finished;
            // Transport Name; Origin Name; Destination Name; Description
            return DatesStartAndFinish
                + "\t" + TransportName + "\t" + OriginName + "\t" + DestinationName + "\t" + Description;
        }
    }
}

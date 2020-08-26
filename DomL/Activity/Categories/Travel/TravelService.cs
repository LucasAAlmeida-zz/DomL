using DomL.Business.DTOs;
using DomL.Business.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DomL.Business.Services
{
    public class TravelService
    {
        public static void SaveFromRawSegments(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            // TRAVEL; Transportation Name; Origin Name; Destination Name; (Description)
            var transportationName = segments[1];
            var originName = segments[2];
            var destinationName = segments[3];
            var description = (segments.Length > 4) ? segments[4] : null;

            Transport transport = TransportService.GetOrCreateByName(transportationName, unitOfWork);
            Location origin = LocationService.GetOrCreateByName(originName, unitOfWork);
            Location destination = LocationService.GetOrCreateByName(destinationName, unitOfWork);

            CreateActivity(activity, transport, origin, destination, description, unitOfWork);
        }

        private static void CreateActivity(Activity activity, Transport transport, Location origin, Location destination, string description, UnitOfWork unitOfWork)
        {
            var travelActivity = new TravelActivity() {
                Activity = activity,
                Transport = transport,
                Origin = origin,
                Destination = destination,
                Description = description
            };

            activity.TravelActivity = travelActivity;

            unitOfWork.TravelRepo.CreateActivity(travelActivity);
        }
    }
}

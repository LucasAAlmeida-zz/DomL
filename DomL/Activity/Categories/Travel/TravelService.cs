using DomL.Business.DTOs;
using DomL.Business.Entities;
using DomL.DataAccess;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace DomL.Business.Services
{
    public class TravelService
    {
        public static void SaveFromRawSegments(string[] rawSegments, Activity activity, UnitOfWork unitOfWork)
        {
            var consolidated = new ConsolidatedTravelDTO(rawSegments, activity);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        public static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            var consolidated = new ConsolidatedTravelDTO(backupSegments);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        private static void SaveFromConsolidated(ConsolidatedTravelDTO consolidated, UnitOfWork unitOfWork)
        {
            var transport = TransportService.CreateOrGetByName(consolidated.TransportName, unitOfWork);
            var origin = LocationService.GetOrCreateByName(consolidated.OriginName, unitOfWork);
            var destination = LocationService.GetOrCreateByName(consolidated.DestinationName, unitOfWork);

            var activity = ActivityService.Create(consolidated, unitOfWork);
            CreateActivity(activity, transport, origin, destination, consolidated.Description, unitOfWork);
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

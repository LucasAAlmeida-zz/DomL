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
            var consolidated = new Consolidated(rawSegments, activity);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        public static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            var consolidated = new Consolidated(backupSegments);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        private static void SaveFromConsolidated(Consolidated consolidated, UnitOfWork unitOfWork)
        {
            var activity = ActivityService.Create(consolidated, unitOfWork);
            CreateActivity(activity, consolidated.Transport, consolidated.Origin, consolidated.Destination, consolidated.Description, unitOfWork);
        }

        private static void CreateActivity(Activity activity, string transport, string origin, string destination, string description, UnitOfWork unitOfWork)
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

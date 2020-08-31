using DomL.Business.Entities;
using DomL.DataAccess;
using System;
using System.IO;
using System.Text.RegularExpressions;

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

        public static void RestoreFromFile(string fileDir)
        {
            using (var reader = new StreamReader(fileDir + "Travel.txt")) {
                string line = "";
                while ((line = reader.ReadLine()) != null) {
                    if (string.IsNullOrWhiteSpace(line)) {
                        continue;
                    }

                    var segments = Regex.Split(line, "\t");

                    // Date; Transportation Name; Origin Name; Destination Name; (Description)
                    var date = segments[0];
                    var transportationName = segments[1];
                    var originName = segments[2];
                    var destinationName = segments[3];
                    var description = segments[4] != "-" ? segments[4] : null;

                    var originalLine = "TRAVEL; " + transportationName + "; " + originName + "; " + destinationName;
                    originalLine = (!string.IsNullOrWhiteSpace(description)) ? originalLine + "; " + description : originalLine;

                    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                        Transport transport = TransportService.GetOrCreateByName(transportationName, unitOfWork);
                        Location origin = LocationService.GetOrCreateByName(originName, unitOfWork);
                        Location destination = LocationService.GetOrCreateByName(destinationName, unitOfWork);

                        var statusSingle = unitOfWork.ActivityRepo.GetStatusById(ActivityStatus.SINGLE);
                        var category = unitOfWork.ActivityRepo.GetCategoryById(ActivityCategory.TRAVEL_ID);
                        var dateDT = DateTime.ParseExact(date, "dd/MM/yy", null);
                        var activity = ActivityService.Create(dateDT, 0, statusSingle, category, null, originalLine, unitOfWork);

                        CreateActivity(activity, transport, origin, destination, description, unitOfWork);

                        unitOfWork.Complete();
                    }
                }
            }
        }
    }
}

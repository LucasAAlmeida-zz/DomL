using DomL.Business.DTOs;
using DomL.Business.Entities;
using DomL.DataAccess;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace DomL.Business.Services
{
    public class EventService
    {
        public static void SaveFromRawSegments(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            // Description
            var description = segments[0];
            var isImportant = false;

            if (description.StartsWith("*")) {
                isImportant = true;
                description = description.Substring(1);
            }

            CreateEventActivity(activity, description, isImportant, unitOfWork);
        }

        public static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            var consolidated = new ConsolidatedEventDTO(backupSegments);

            var activity = ActivityService.Create(consolidated, unitOfWork);
            CreateEventActivity(activity, consolidated.Description, consolidated.IsImportant, unitOfWork);
        }

        private static void CreateEventActivity(Activity activity, string description, bool isImportant, UnitOfWork unitOfWork)
        {
            var eventActivity = new EventActivity() {
                Activity = activity,
                Description = description,
                IsImportant = isImportant
            };

            activity.EventActivity = eventActivity;

            unitOfWork.EventRepo.CreateEventActivity(eventActivity);
        }

        public static void RestoreFromFile(string fileDir)
        {
            using (var reader = new StreamReader(fileDir + "Event.txt")) {
                string line = "";
                while ((line = reader.ReadLine()) != null) {
                    if (string.IsNullOrWhiteSpace(line)) {
                        continue;
                    }

                    var segments = Regex.Split(line, "\t");

                    // Date; Description
                    var date = segments[0];
                    var description = segments[1];

                    var originalLine = "*" + description;

                    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                        var statusSingle = unitOfWork.ActivityRepo.GetStatusById(ActivityStatus.SINGLE);
                        var category = unitOfWork.ActivityRepo.GetCategoryById(ActivityCategory.EVENT_ID);
                        
                        var dateDT = DateTime.ParseExact(date, "dd/MM/yy", null);
                        var activity = ActivityService.Create(dateDT, 0, statusSingle, category, null, originalLine, unitOfWork);
                        CreateEventActivity(activity, description, true, unitOfWork);

                        unitOfWork.Complete();
                    }
                }
            }
        }
    }
}

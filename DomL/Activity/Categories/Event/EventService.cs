using DomL.Business.DTOs;
using DomL.Business.Entities;
using DomL.Business.Utils;
using DomL.DataAccess;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace DomL.Business.Services
{
    public class EventService
    {
        public static void SaveFromRawSegments(string[] rawSegments, Activity activity, UnitOfWork unitOfWork)
        {
            var consolidated = new EventConsolidatedDTO(rawSegments, activity);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        public static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            var consolidated = new EventConsolidatedDTO(backupSegments);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        private static void SaveFromConsolidated(EventConsolidatedDTO consolidated, UnitOfWork unitOfWork)
        {
            var activity = ActivityService.Create(consolidated, unitOfWork);
            CreateEventActivity(activity, consolidated.Description, consolidated.IsImportant, unitOfWork);
        }

        private static void CreateEventActivity(Activity activity, string description, bool isImportant, UnitOfWork unitOfWork)
        {
            var eventActivity = new EventActivity() {
                Activity = activity,
                Description = Util.GetStringOrNull(description),
                IsImportant = isImportant
            };

            activity.EventActivity = eventActivity;

            unitOfWork.EventRepo.CreateEventActivity(eventActivity);
        }
    }
}

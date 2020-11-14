using DomL.Business.DTOs;
using DomL.Business.Entities;
using DomL.DataAccess;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace DomL.Business.Services
{
    public class MeetService
    {
        public static void SaveFromRawSegments(string[] rawSegments, Activity activity, UnitOfWork unitOfWork)
        {
            var consolidated = new MeetConsolidatedDTO(rawSegments, activity);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        public static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            var consolidated = new MeetConsolidatedDTO(backupSegments);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        private static void SaveFromConsolidated(MeetConsolidatedDTO consolidated, UnitOfWork unitOfWork)
        {
            var person = PersonService.CreatePerson(consolidated.Person, unitOfWork);

            var activity = ActivityService.Create(consolidated, unitOfWork);
            CreateMeetActivity(activity, consolidated.Person, consolidated.Origin, consolidated.Description, unitOfWork);
        }

        private static void CreateMeetActivity(Activity activity, string name, string origin, string description, UnitOfWork unitOfWork)
        {
            var meetActivity = new MeetActivity() {
                Activity = activity,
                Person = name,
                Origin = origin,
                Description = description
            };

            activity.MeetActivity = meetActivity;

            unitOfWork.MeetRepo.CreateMeetActivity(meetActivity);
        }
    }
}

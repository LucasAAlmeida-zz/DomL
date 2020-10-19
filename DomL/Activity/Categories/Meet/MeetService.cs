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
            var consolidated = new ConsolidatedMeetDTO(rawSegments, activity);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        public static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            var consolidated = new ConsolidatedMeetDTO(backupSegments);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        private static void SaveFromConsolidated(ConsolidatedMeetDTO consolidated, UnitOfWork unitOfWork)
        {
            var person = PersonService.CreatePerson(consolidated.PersonName, unitOfWork);

            var activity = ActivityService.Create(consolidated, unitOfWork);
            CreateMeetActivity(activity, person, consolidated.Origin, consolidated.Description, unitOfWork);
        }

        private static void CreateMeetActivity(Activity activity, Person person, string origin, string description, UnitOfWork unitOfWork)
        {
            var meetActivity = new MeetActivity() {
                Activity = activity,
                Person = person,
                Origin = origin,
                Description = description
            };

            activity.MeetActivity = meetActivity;

            unitOfWork.MeetRepo.CreateMeetActivity(meetActivity);
        }
    }
}

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

        public static void RestoreFromFile(string fileDir)
        {
            using (var reader = new StreamReader(fileDir + "Meet.txt")) {
                string line = "";
                while ((line = reader.ReadLine()) != null) {
                    if (string.IsNullOrWhiteSpace(line)) {
                        continue;
                    }

                    var segments = Regex.Split(line, "\t");

                    // Date; Person Name; Origin; Description
                    var date = segments[0];
                    var personName = segments[1];
                    var origin = segments[2];
                    var description = segments[3] != "-" ? segments[3] : null;

                    var originalLine = "MEET; " + personName + "; " + origin;
                    originalLine = (!string.IsNullOrWhiteSpace(description)) ? originalLine + "; " + description : originalLine;

                    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                        var person = PersonService.CreatePerson(personName, unitOfWork);
                        var statusSingle = unitOfWork.ActivityRepo.GetStatusById(ActivityStatus.SINGLE);
                        var category = unitOfWork.ActivityRepo.GetCategoryById(ActivityCategory.MEET_ID);

                        var dateDT = DateTime.ParseExact(date, "dd/MM/yy", null);
                        var activity = ActivityService.Create(dateDT, 0, statusSingle, category, null, originalLine, unitOfWork);

                        CreateMeetActivity(activity, person, origin, description, unitOfWork);

                        unitOfWork.Complete();
                    }
                }
            }
        }
    }
}

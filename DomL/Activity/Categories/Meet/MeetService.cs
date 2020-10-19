﻿using DomL.Business.Entities;
using DomL.DataAccess;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace DomL.Business.Services
{
    public class MeetService
    {
        public static void SaveFromRawSegments(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            // MEET; Person Name; Origin; Description
            var personName = segments[1];
            var origin = segments[2];
            var description = segments[3];

            Person person = PersonService.CreatePerson(personName, unitOfWork);

            CreateMeetActivity(activity, person, origin, description, unitOfWork);
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

        internal static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            throw new NotImplementedException();
        }
    }
}

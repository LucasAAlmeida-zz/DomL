using DomL.Business.Entities;
using DomL.DataAccess;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace DomL.Business.Services
{
    public class PlayService
    {
        public static void SaveFromRawSegments(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            // PLAY; Who; (Description)
            var who = segments[1];
            var description = segments.Length > 2 ? segments[2] : null;

            CreatePlayActivity(activity, who, description, unitOfWork);
        }

        private static void CreatePlayActivity(Activity activity, string who, string description, UnitOfWork unitOfWork)
        {
            var playActivity = new PlayActivity() {
                Activity = activity,
                Who = who,
                Description = description
            };

            activity.PlayActivity = playActivity;

            unitOfWork.PlayRepo.CreatePlayActivity(playActivity);
        }

        public static void RestoreFromFile(string fileDir)
        {
            using (var reader = new StreamReader(fileDir + "Play.txt")) {
                string line = "";
                while ((line = reader.ReadLine()) != null) {
                    if (string.IsNullOrWhiteSpace(line)) {
                        continue;
                    }

                    var segments = Regex.Split(line, "\t");

                    // Date; Work Name; Description
                    var date = segments[0];
                    var who = segments[1];
                    var description = segments[2] != "-" ? segments[2] : null;

                    var originalLine = "PLAY; " + who;
                    originalLine = (!string.IsNullOrWhiteSpace(description)) ? originalLine + "; " + description : originalLine;

                    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                        var statusSingle = unitOfWork.ActivityRepo.GetStatusById(ActivityStatus.SINGLE);
                        var category = unitOfWork.ActivityRepo.GetCategoryById(ActivityCategory.PLAY_ID);

                        var dateDT = DateTime.ParseExact(date, "dd/MM/yy", null);
                        var activity = ActivityService.Create(dateDT, 0, statusSingle, category, null, originalLine, unitOfWork);

                        CreatePlayActivity(activity, who, description, unitOfWork);

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

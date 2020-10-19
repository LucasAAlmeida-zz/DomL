using DomL.Business.DTOs;
using DomL.Business.Entities;
using DomL.DataAccess;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace DomL.Business.Services
{
    public class PlayService
    {
        public static void SaveFromRawSegments(string[] rawSegments, Activity activity, UnitOfWork unitOfWork)
        {
            var consolidated = new ConsolidatedPlayDTO(rawSegments, activity);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        public static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            var consolidated = new ConsolidatedPlayDTO(backupSegments);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        private static void SaveFromConsolidated(ConsolidatedPlayDTO consolidated, UnitOfWork unitOfWork)
        {
            var activity = ActivityService.Create(consolidated, unitOfWork);
            CreatePlayActivity(activity, consolidated.Who, consolidated.Description, unitOfWork);
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
    }
}

using DomL.Business.Entities;
using DomL.DataAccess;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace DomL.Business.Services
{
    public class GiftService
    {
        public static void SaveFromRawSegments(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            // GIFT; Gift; Is To or From; Who; (Description)
            var gift = segments[1];
            var isFrom = segments[2].ToLower() == "from";
            var who = segments[3];
            var description = (segments.Length > 4) ? segments[4] : null;

            CreateGiftActivity(activity, gift, isFrom, who, description, unitOfWork);
        }

        private static void CreateGiftActivity(Activity activity, string gift, bool isFrom, string who, string description, UnitOfWork unitOfWork)
        {
            var giftActivity = new GiftActivity() {
                Activity = activity,
                Gift = gift,
                IsFrom = isFrom,
                Who = who,
                Description = description
            };

            activity.GiftActivity = giftActivity;

            unitOfWork.GiftRepo.CreateGiftActivity(giftActivity);
        }

        public static void RestoreFromFile(string fileDir)
        {
            using (var reader = new StreamReader(fileDir + "Gift.txt")) {
                string line = "";
                while ((line = reader.ReadLine()) != null) {
                    if (string.IsNullOrWhiteSpace(line)) {
                        continue;
                    }

                    var segments = Regex.Split(line, "\t");

                    // Date; Gift; Is To or From; Who; (Description)
                    var date = segments[0];
                    var gift = segments[1];
                    var toOrFrom = segments[2];
                    var who = segments[3];
                    var description = segments[4] != "-" ? segments[4] : null;

                    var originalLine = "GIFT; " + gift + "; " + toOrFrom + "; " + who;
                    originalLine = (!string.IsNullOrWhiteSpace(description)) ? originalLine + "; " + description : originalLine;

                    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                        var statusSingle = unitOfWork.ActivityRepo.GetStatusById(ActivityStatus.SINGLE);
                        var category = unitOfWork.ActivityRepo.GetCategoryById(ActivityCategory.GIFT_ID);

                        var dateDT = DateTime.ParseExact(date, "dd/MM/yy", null);
                        var activity = ActivityService.Create(dateDT, 0, statusSingle, category, null, originalLine, unitOfWork);

                        var isFrom = toOrFrom.ToLower() == "from";
                        CreateGiftActivity(activity, gift, isFrom, who, description, unitOfWork);

                        unitOfWork.Complete();
                    }
                }
            }
        }
    }
}

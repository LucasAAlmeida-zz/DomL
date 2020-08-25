using DomL.Business.DTOs;
using DomL.Business.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DomL.Business.Services
{
    public class GiftService
    {
        public static void SaveFromRawSegments(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            // GIFT; Gift; Is To or From; Person; (Description)
            var gift = segments[1];
            var isFrom = segments[2].ToLower() == "from";
            var personName = segments[3];
            var description = (segments.Length > 4) ? segments[4] : null;

            Person person = PersonService.GetOrCreateByName(personName, unitOfWork);

            CreateGiftActivity(activity, gift, isFrom, person, description, unitOfWork);
        }

        private static void CreateGiftActivity(Activity activity, string gift, bool isFrom, Person person, string description, UnitOfWork unitOfWork)
        {
            var giftActivity = new GiftActivity() {
                Activity = activity,
                Gift = gift,
                IsFrom = isFrom,
                Person = person,
                Description = description
            };

            activity.GiftActivity = giftActivity;
            activity.PairUpActivity(unitOfWork);

            unitOfWork.GiftRepo.CreateGiftActivity(giftActivity);
        }

        public static IEnumerable<Activity> GetStartingActivity(IQueryable<Activity> previousStartingActivities, Activity activity)
        {
            var gift = activity.GiftActivity.Gift;
            return previousStartingActivities.Where(u =>
                u.CategoryId == ActivityCategory.GIFT
                && u.GiftActivity.Gift == gift
            );
        }
    }
}

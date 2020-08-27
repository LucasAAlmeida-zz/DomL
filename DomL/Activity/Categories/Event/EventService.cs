using DomL.Business.Entities;

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
    }
}

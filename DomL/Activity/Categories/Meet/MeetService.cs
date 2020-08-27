using DomL.Business.Entities;

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
    }
}

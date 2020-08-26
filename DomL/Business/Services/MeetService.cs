using DomL.Business.DTOs;
using DomL.Business.Entities;
using System.Collections.Generic;
using System.Linq;

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

            Person person = PersonService.GetOrCreateByNameAndOrigin(personName, origin, unitOfWork);

            CreateMeetActivity(activity, person, description, unitOfWork);
        }

        private static void CreateMeetActivity(Activity activity, Person person, string description, UnitOfWork unitOfWork)
        {
            var meetActivity = new MeetActivity() {
                Activity = activity,
                Person = person,
                Description = description
            };

            activity.MeetActivity = meetActivity;

            unitOfWork.MeetRepo.CreateMeetActivity(meetActivity);
        }
    }
}

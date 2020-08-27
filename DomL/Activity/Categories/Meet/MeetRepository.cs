using DomL.Business.Entities;

namespace DomL.DataAccess
{
    public class MeetRepository : DomLRepository<MeetActivity>
    {
        public MeetRepository(DomLContext context) : base(context) { }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }

        public void CreateMeetActivity(MeetActivity meetActivity)
        {
            DomLContext.MeetActivity.Add(meetActivity);
        }
    }
}

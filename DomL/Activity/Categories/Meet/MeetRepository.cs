using DomL.Business.Entities;
using System;
using System.Linq;

namespace DomL.DataAccess
{
    public class MeetRepository : BaseRepository<MeetActivity>
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

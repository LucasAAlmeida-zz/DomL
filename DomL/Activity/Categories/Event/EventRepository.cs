using DomL.Business.Entities;

namespace DomL.DataAccess
{
    public class EventRepository : DomLRepository<EventActivity>
    {
        public EventRepository(DomLContext context) : base(context) { }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }

        public void CreateEventActivity(EventActivity eventActivity)
        {
            DomLContext.EventActivity.Add(eventActivity);
        }
    }
}

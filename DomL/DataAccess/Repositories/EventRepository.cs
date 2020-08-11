using DomL.Business.Activities.SingleDayActivities;

namespace DomL.DataAccess
{
    public class EventRepository : BaseRepository<Event>
    {
        public EventRepository(DomLContext context)
        : base(context)
        {
        }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }
    }
}

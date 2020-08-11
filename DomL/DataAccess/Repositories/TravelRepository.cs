using DomL.Business.Activities.SingleDayActivities;

namespace DomL.DataAccess
{
    public class TravelRepository : BaseRepository<Travel>
    {
        public TravelRepository(DomLContext context)
        : base(context)
        {
        }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }
    }
}

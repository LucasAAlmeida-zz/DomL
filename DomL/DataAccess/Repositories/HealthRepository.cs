using DomL.Business.Activities.SingleDayActivities;

namespace DomL.DataAccess
{
    public class HealthRepository : BaseRepository<Health>
    {
        public HealthRepository(DomLContext context)
        : base(context)
        {
        }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }
    }
}

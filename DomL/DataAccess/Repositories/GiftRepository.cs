using DomL.Business.Activities.SingleDayActivities;

namespace DomL.DataAccess
{
    public class GiftRepository : BaseRepository<Gift>
    {
        public GiftRepository(DomLContext context)
        : base(context)
        {
        }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }
    }
}

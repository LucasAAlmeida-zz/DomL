using DomL.Business.Entities;

namespace DomL.DataAccess
{
    public class TravelRepository : DomLRepository<TravelActivity>
    {
        public TravelRepository(DomLContext context) : base(context) { }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }

        public void CreateActivity(TravelActivity travelActivity)
        {
            DomLContext.TravelActivity.Add(travelActivity);
        }
    }
}

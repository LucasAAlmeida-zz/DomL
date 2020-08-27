
using DomL.Business.Entities;
using System.Linq;

namespace DomL.DataAccess
{
    public class HealthRepository : DomLRepository<HealthActivity>
    {
        public HealthRepository(DomLContext context) : base(context) { }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }

        public void CreateHealthActivity(HealthActivity healthActivity)
        {
            DomLContext.HealthActivity.Add(healthActivity);
        }
    }
}

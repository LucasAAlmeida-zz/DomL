using DomL.Business.Entities;

namespace DomL.DataAccess
{
    public class WorkRepository : DomLRepository<WorkActivity>
    {
        public WorkRepository(DomLContext context) : base(context) { }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }

        public void CreateWorkActivity(WorkActivity workActivity)
        {
            DomLContext.WorkActivity.Add(workActivity);
        }
    }
}

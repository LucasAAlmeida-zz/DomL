using DomL.Business.Entities;

namespace DomL.DataAccess
{
    public class DoomRepository : DomLRepository<DoomActivity>
    {
        public DoomRepository(DomLContext context) : base(context) { }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }

        public void CreateDoomActivity(DoomActivity doomActivity)
        {
            DomLContext.DoomActivity.Add(doomActivity);
        }
    }
}

using DomL.Business.Entities;
using System.Linq;

namespace DomL.DataAccess
{
    public class DoomRepository : BaseRepository<DoomActivity>
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

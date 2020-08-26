using DomL.Business.Entities;
using System;
using System.Linq;

namespace DomL.DataAccess
{
    public class PlayRepository : BaseRepository<PlayActivity>
    {
        public PlayRepository(DomLContext context) : base(context) { }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }

        public void CreatePlayActivity(PlayActivity playActivity)
        {
            DomLContext.PlayActivity.Add(playActivity);
        }
    }
}

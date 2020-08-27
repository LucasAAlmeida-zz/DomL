using DomL.Business.Entities;

namespace DomL.DataAccess
{
    public class PlayRepository : DomLRepository<PlayActivity>
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

using DomL.Business.Entities;

namespace DomL.DataAccess.Repositories
{
    public class ScoreRepository : DomLRepository<Score>
    {
        public ScoreRepository(DomLContext context) : base(context) { }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }
    }
}

using DomL.Business.Entities;

namespace DomL.DataAccess.Repositories
{
    public class MediaTypeRepository : BaseRepository<MediaType>
    {
        public MediaTypeRepository(DomLContext context) : base(context) { }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }
    }
}

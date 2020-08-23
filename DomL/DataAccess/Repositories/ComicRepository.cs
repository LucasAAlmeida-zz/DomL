using DomL.Business.Entities;
using System.Linq;

namespace DomL.DataAccess
{
    public class ComicRepository : BaseRepository<ComicVolume>
    {
        public ComicRepository(DomLContext context) : base(context) { }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }

        public ComicVolume GetComicVolumeByChapters(string chapters)
        {
            return DomLContext.ComicVolume.SingleOrDefault(u => u.Chapters == chapters);
        }

        public void CreateComicActivity(ComicActivity comicActivity)
        {
            DomLContext.ComicActivity.Add(comicActivity);
        }
    }
}

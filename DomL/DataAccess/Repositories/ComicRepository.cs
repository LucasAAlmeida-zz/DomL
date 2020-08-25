using DomL.Business.Entities;
using DomL.Business.Utils;
using System.Linq;
using System.Data.Entity;

namespace DomL.DataAccess
{
    public class ComicRepository : BaseRepository<ComicActivity>
    {
        public ComicRepository(DomLContext context) : base(context) { }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }

        public ComicVolume GetComicVolumeBySeriesNameAndChapters(string seriesName, string chapters)
        {
            var cleanSeriesName = Util.CleanString(seriesName);
            return DomLContext.ComicVolume
                .Include(u => u.Author)
                .Include(u => u.Series)
                .Include(u => u.Type)
                .SingleOrDefault(u => 
                    u.Series.Name.Replace(":", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").Replace("'", "").Replace(",", "").ToLower().Replace("the", "")
                    == cleanSeriesName
                    && u.Chapters == chapters
                );
        }

        public void CreateComicActivity(ComicActivity comicActivity)
        {
            DomLContext.ComicActivity.Add(comicActivity);
        }

        public void CreateComicVolume(ComicVolume comicVolume)
        {
            DomLContext.ComicVolume.Add(comicVolume);
        }
    }
}

using DomL.Business.Entities;
using DomL.Business.Utils;
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

        public ComicVolume GetComicVolumeBySeriesNameAndChapters(string seriesName, string chapters)
        {
            var cleanSeriesName = Util.CleanString(seriesName);
            return DomLContext.ComicVolume.SingleOrDefault(u => 
                u.Series.Name.Replace(":", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").Replace("'", "").Replace(",", "").ToLower().Replace("the", "")
                == cleanSeriesName
                && u.Chapters == chapters
            );
        }

        public void CreateComicActivity(ComicActivity comicActivity)
        {
            DomLContext.ComicActivity.Add(comicActivity);
        }
    }
}

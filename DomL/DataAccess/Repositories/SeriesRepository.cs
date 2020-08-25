using DomL.Business.Entities;
using DomL.Business.Utils;
using System.Linq;

namespace DomL.DataAccess
{
    public class SeriesRepository : BaseRepository<Series>
    {
        public SeriesRepository(DomLContext context) : base(context) { }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }

        public Series GetByName(string seriesName)
        {
            var cleanSeriesName = Util.CleanString(seriesName);
            return DomLContext.Series.SingleOrDefault(u =>
                u.Name.Replace(":", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").Replace("'", "").Replace(",", "").ToLower().Replace("the", "")
                == cleanSeriesName
            );
        }
    }
}

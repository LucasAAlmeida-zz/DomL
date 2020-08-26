using DomL.Business.Entities;
using DomL.Business.Utils;
using System.Linq;
using System.Data.Entity;

namespace DomL.DataAccess
{
    public class ShowRepository : DomLRepository<ShowActivity>
    {
        public ShowRepository(DomLContext context) : base(context) { }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }

        public ShowSeason GetShowSeasonBySeriesNameAndSeason(string seriesName, string season)
        {
            var cleanSeriesName = Util.CleanString(seriesName);
            return DomLContext.ShowSeason
                .Include(u => u.Director)
                .Include(u => u.Series)
                .Include(u => u.Type)
                .SingleOrDefault(u => 
                    u.Series.Name.Replace(":", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").Replace("'", "").Replace(",", "").ToLower().Replace("the", "")
                    == cleanSeriesName
                    && u.Season == season
                );
        }

        public void CreateShowActivity(ShowActivity showActivity)
        {
            DomLContext.ShowActivity.Add(showActivity);
        }

        public void CreateShowSeason(ShowSeason showSeason)
        {
            DomLContext.ShowSeason.Add(showSeason);
        }
    }
}

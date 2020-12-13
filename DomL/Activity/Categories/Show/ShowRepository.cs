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

        public Show GetShowByTitle(string title)
        {
            var cleanTitle = Util.CleanString(title);
            return DomLContext.ShowSeason
                .Include(u => u.Series)
                .SingleOrDefault(u => 
                    u.Title.Replace(":", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").Replace("'", "").Replace(",", "").ToLower().Replace("the", "")
                    == cleanTitle
                );
        }

        public void CreateShowActivity(ShowActivity showActivity)
        {
            DomLContext.ShowActivity.Add(showActivity);
        }

        public void CreateShowSeason(Show showSeason)
        {
            DomLContext.ShowSeason.Add(showSeason);
        }
    }
}

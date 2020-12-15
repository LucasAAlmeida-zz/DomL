using DomL.Business.Entities;
using DomL.Business.Utils;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

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
            return DomLContext.Show
                .SingleOrDefault(u => 
                    u.Title.Replace(":", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").Replace("'", "").Replace(",", "").ToLower().Replace("the", "")
                    == cleanTitle
                );
        }

        public void CreateShowActivity(ShowActivity showActivity)
        {
            DomLContext.ShowActivity.Add(showActivity);
        }

        public void CreateShow(Show show)
        {
            DomLContext.Show.Add(show);
        }

        public List<Show> GetAllShows()
        {
            return DomLContext.Show.ToList();
        }
    }
}

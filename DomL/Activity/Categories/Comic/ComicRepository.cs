using DomL.Business.Entities;
using DomL.Business.Utils;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

namespace DomL.DataAccess
{
    public class ComicRepository : DomLRepository<ComicActivity>
    {
        public ComicRepository(DomLContext context) : base(context) { }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }

        public Comic GetComicByTitle(string title)
        {
            var cleanTitle = Util.CleanString(title);
            return DomLContext.Comic.SingleOrDefault(u => u.Title == title);
        }

        public void CreateComicActivity(ComicActivity comicActivity)
        {
            DomLContext.ComicActivity.Add(comicActivity);
        }

        public void CreateComicVolume(Comic comic)
        {
            DomLContext.Comic.Add(comic);
        }

        public List<Comic> GetAllComics()
        {
            return DomLContext.Comic.ToList();
        }
    }
}

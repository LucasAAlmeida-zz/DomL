using DomL.Business.Entities;
using System;
using System.Linq;

namespace DomL.DataAccess
{
    public class ComicRepository : BaseRepository<ComicVolume>
    {
        public ComicRepository(DomLContext context)
        : base(context)
        {
        }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }

        public ComicSeries GetSeriesByName(string name)
        {
            return DomLContext.ComicSeries.SingleOrDefault(bs => bs.Name == name);
        }
        
        public ComicAuthor GetAuthorByName(string name)
        {
            return DomLContext.ComicAuthor.SingleOrDefault(ba => ba.Name == name);
        }

        public ComicType GetTypeByName(string name)
        {
            return DomLContext.ComicType.SingleOrDefault(u => u.Name == name);
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

using DomL.Business.Entities;
using DomL.Business.Utils;
using System;
using System.Linq;

namespace DomL.DataAccess.Repositories
{
    public class MediaTypeRepository : BaseRepository<MediaType>
    {
        public MediaTypeRepository(DomLContext context) : base(context) { }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }

        public MediaType GetByName(string typeName)
        {
            var cleanTypeName = Util.CleanString(typeName);
            return DomLContext.MediaType.SingleOrDefault(u =>
                u.Name.Replace(":", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").Replace("'", "").Replace(",", "").ToLower().Replace("the", "")
                == cleanTypeName
            );
        }
    }
}

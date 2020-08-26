using DomL.Business.Entities;
using DomL.Business.Utils;
using System.Linq;

namespace DomL.DataAccess
{
    public class PersonRepository : BaseRepository<Person>
    {
        public PersonRepository(DomLContext context) : base(context) { }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }

        public Person GetByName(string personName)
        {
            var cleanPersonName = Util.CleanString(personName);
            return DomLContext.Person.SingleOrDefault(u =>
                u.Name.Replace(":", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").Replace("'", "").Replace(",", "").ToLower().Replace("the", "")
                == cleanPersonName
            );
        }

        internal Person GetByNameAndOrigin(string personName, string origin)
        {
            var cleanPersonName = Util.CleanString(personName);
            var cleanOrigin = Util.CleanString(origin);
            return DomLContext.Person.SingleOrDefault(u =>
                u.Name.Replace(":", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").Replace("'", "").Replace(",", "").ToLower().Replace("the", "")
                == cleanPersonName
                && u.Origin.Replace(":", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").Replace("'", "").Replace(",", "").ToLower().Replace("the", "")
                == cleanOrigin
            );
        }
    }

}

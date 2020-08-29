using DomL.Business.Entities;
using DomL.Business.Utils;
using System.Collections.Generic;
using System.Linq;

namespace DomL.DataAccess
{
    public class PersonRepository : DomLRepository<Person>
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
    }
}

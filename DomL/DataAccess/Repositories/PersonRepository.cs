using DomL.Business.Entities;

namespace DomL.DataAccess
{
    public class PersonRepository : BaseRepository<Person>
    {
        public PersonRepository(DomLContext context) : base(context) { }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }
    }

}

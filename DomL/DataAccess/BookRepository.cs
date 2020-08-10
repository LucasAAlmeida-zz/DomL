using DomL.Business.Activities.MultipleDayActivities;

namespace DomL.DataAccess
{
    public class BookRepository : Repository<Book>
    {
        public BookRepository(DomLContext context)
        : base(context)
        {
        }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }

    }
}

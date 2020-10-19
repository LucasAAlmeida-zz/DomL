using DomL.Business.Entities;
using DomL.Business.Utils;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

namespace DomL.DataAccess
{
    public class BookRepository : DomLRepository<BookActivity>
    {
        public BookRepository(DomLContext context) : base(context) { }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }

        public Book GetBookByTitle(string title)
        {
            var cleanTitle = Util.CleanString(title);
            return DomLContext.Book
                .Include(u => u.Author)
                .Include(u => u.Series)
                .SingleOrDefault(u =>
                    u.Title.Replace(":", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").Replace("'", "").Replace(",", "").ToLower().Replace("the", "")
                    == cleanTitle
                );
        }

        public void CreateBookActivity(BookActivity bookActivity)
        {
            DomLContext.BookActivity.Add(bookActivity);
        }

        public void CreateBook(Book book)
        {
            DomLContext.Book.Add(book);
        }

        public List<Book> GetAllBooks()
        {
            return DomLContext.Book.ToList();
        }
    }
}

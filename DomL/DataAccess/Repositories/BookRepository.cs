using DomL.Business.Entities;
using System;
using System.Linq;

namespace DomL.DataAccess
{
    public class BookRepository : BaseRepository<Book>
    {
        public BookRepository(DomLContext context)
        : base(context)
        {
        }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }

        public BookAuthor GetAuthorByName(string name)
        {
            return DomLContext.BookAuthor.SingleOrDefault(ba => ba.Name == name);
        }

        public BookSeries GetSeriesByName(string name)
        {
            return DomLContext.BookSeries.SingleOrDefault(bs => bs.Name == name);
        }

        public Book GetBookByTitle(string title)
        {
            return DomLContext.Book.SingleOrDefault(b => b.Title == title);
        }

        public void CreateBookActivity(BookActivity bookActivity)
        {
            DomLContext.BookActivity.Add(bookActivity);
        }
    }
}

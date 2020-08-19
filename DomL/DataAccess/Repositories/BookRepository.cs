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

        public BookAuthor CreateAuthor(BookAuthor author)
        {
            author = DomLContext.BookAuthor.Add(author);
            DomLContext.SaveChanges();
            return author;
        }

        public BookSeries GetSeriesByName(string name)
        {
            return DomLContext.BookSeries.SingleOrDefault(bs => bs.Name == name);
        }

        public BookSeries CreateSeries(BookSeries series)
        {
            series = DomLContext.BookSeries.Add(series);
            DomLContext.SaveChanges();
            return series;
        }

        public Book GetBookByTitle(string title)
        {
            return DomLContext.Book.SingleOrDefault(b => b.Title == title);
        }

        public Book CreateBook(Book book)
        {
            book = DomLContext.Book.Add(book);
            DomLContext.SaveChanges();
            return book;
        }

        public BookActivity CreateBookActivity(BookActivity bookActivity)
        {
            bookActivity = DomLContext.BookActivity.Add(bookActivity);
            DomLContext.SaveChanges();
            return bookActivity;
        }
    }
}

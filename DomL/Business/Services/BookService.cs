using DomL.Business.Entities;
using DomL.Business.Utils.Enums;
using DomL.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomL.Business.Services
{
    public class BookService
    {
        public static void SaveFromRawLine(string[] segmentos, Activity activity)
        {
            // BOOK; Author Name; Series Name; Title; Number In Series; Score; Description

            var authorName = segmentos[1];
            var seriesName = segmentos[2];
            var bookTitle = segmentos[3];
            var numberInSeries = int.Parse(segmentos[4]);
            var score = int.Parse(segmentos[5]);
            var description = segmentos[6];

            var bookRepo = new BookRepository(new DomLContext());

            var author = bookRepo.GetAuthorByName(authorName);
            if (author == null) {
                author = new BookAuthor() {
                    Name = authorName
                };
                author = bookRepo.CreateAuthor(author);
            }

            var series = bookRepo.GetSeriesByName(seriesName);
            if (series == null) {
                series = new BookSeries() {
                    Name = seriesName
                };
                series = bookRepo.CreateSeries(series);
            }

            var book = bookRepo.GetBookByTitle(bookTitle);
            if (book == null) {
                book = new Book() {
                    AuthorId = author.Id,
                    SeriesId = series.Id,
                    Title = bookTitle,
                    NumberInSeries = numberInSeries,
                    Score = score,
                };
                book = bookRepo.CreateBook(book);
            }

            var bookActivity = new BookActivity() {
                Id = activity.Id,
                BookId = book.Id,
                Description = description
            };
            bookRepo.CreateBookActivity(bookActivity);
        }
    }
}

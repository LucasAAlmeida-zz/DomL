using DomL.Business.Entities;
using DomL.Business.Utils.Enums;
using DomL.DataAccess;
using DomL.Presentation;
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
            // BOOK; Title; Author Name; Series Name; Number In Series; Score; Description

            segmentos[0] = "";
            var bookWindow = new BookWindow(segmentos);
            bookWindow.ShowDialog();

            var bookTitle = (string) bookWindow.TitleCB.SelectedItem;
            var authorName = (string) bookWindow.AuthorCB.SelectedItem;
            var seriesName = (string) bookWindow.SeriesCB.SelectedItem;
            var numberInSeries = (string) bookWindow.NumberCB.SelectedItem;
            var score = (string) bookWindow.ScoreCB.SelectedItem;
            var description = (string) bookWindow.DescriptionCB.SelectedItem;

            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var author = unitOfWork.BookRepo.GetAuthorByName(authorName);
                if (author == null) {
                    author = new BookAuthor() {
                        Name = authorName
                    };
                }

                var series = unitOfWork.BookRepo.GetSeriesByName(seriesName);
                if (series == null) {
                    series = new BookSeries() {
                        Name = seriesName
                    };
                }

                var book = unitOfWork.BookRepo.GetBookByTitle(bookTitle);
                if (book == null) {
                    book = new Book() {
                        AuthorId = author.Id,
                        SeriesId = series.Id,
                        Title = bookTitle,
                        NumberInSeries = numberInSeries,
                        Score = score,
                    };
                }

                var bookActivity = new BookActivity() {
                    Id = activity.Id,
                    BookId = book.Id,
                    Description = description
                };

                unitOfWork.BookRepo.CreateBookActivity(bookActivity);
            }
        }
    }
}

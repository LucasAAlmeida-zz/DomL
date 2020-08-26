using DomL.Business.DTOs;
using DomL.Business.Entities;
using DomL.Presentation;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;

namespace DomL.Business.Services
{
    public class BookService
    {
        public static void SaveFromRawSegments(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            // BOOK (Classification); Title; (Author Name); (Series Name); (Number In Series); (Score); (Description)
            segments[0] = "";
            var bookWindow = new BookWindow(segments, activity, unitOfWork);

            if (ConfigurationManager.AppSettings["ShowCategoryWindows"] == "true") {
                bookWindow.ShowDialog();
            }

            var bookTitle = bookWindow.TitleCB.Text;
            var authorName = bookWindow.AuthorCB.Text;
            var seriesName = bookWindow.SeriesCB.Text;
            var numberInSeries = (!string.IsNullOrWhiteSpace(bookWindow.NumberCB.Text)) ? bookWindow.NumberCB.Text : null;
            var score = (!string.IsNullOrWhiteSpace(bookWindow.ScoreCB.Text)) ? bookWindow.ScoreCB.Text : null;
            var description = (!string.IsNullOrWhiteSpace(bookWindow.DescriptionCB.Text)) ? bookWindow.DescriptionCB.Text : null;

            Person author = PersonService.GetOrCreateByName(authorName, unitOfWork);
            Series series = SeriesService.GetOrCreateByName(seriesName, unitOfWork);

            Book book = GetOrUpdateOrCreateBook(bookTitle, author, series, numberInSeries, score, unitOfWork);

            CreateBookActivity(activity, book, description, unitOfWork);
        }

        private static void CreateBookActivity(Activity activity, Book book, string description, UnitOfWork unitOfWork)
        {
            var bookActivity = new BookActivity() {
                Activity = activity,
                Book = book,
                Description = description
            };

            activity.BookActivity = bookActivity;
            activity.PairUpActivity(unitOfWork);

            unitOfWork.BookRepo.CreateBookActivity(bookActivity);
        }

        private static Book GetOrUpdateOrCreateBook(string bookTitle, Person author, Series series, string numberInSeries, string score, UnitOfWork unitOfWork)
        {
            var book = unitOfWork.BookRepo.GetBookByTitle(bookTitle);

            if (book == null) {
                book = new Book() {
                    Author = author,
                    Series = series,
                    Title = bookTitle,
                    NumberInSeries = numberInSeries,
                    Score = score,
                };
                unitOfWork.BookRepo.CreateBook(book);
            } else {
                book.Author = author ?? book.Author;
                book.Series = series ?? book.Series;
            }

            return book;
        }

        public static Book GetByTitle(string title, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(title)) {
                return null;
            }
            return unitOfWork.BookRepo.GetBookByTitle(title);
        }

        public static IEnumerable<Activity> GetStartingActivity(IQueryable<Activity> previousStartingActivities, Activity activity)
        {
            var book = activity.BookActivity.Book;
            return previousStartingActivities.Where(u =>
                u.CategoryId == ActivityCategory.BOOK
                && u.BookActivity.Book.Title == book.Title
            );
        }
    }
}

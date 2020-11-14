using DomL.Business.Entities;
using DomL.Presentation;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using DomL.DataAccess;
using DomL.Business.DTOs;
using System.IO;
using System.Text.RegularExpressions;
using System;

namespace DomL.Business.Services
{
    public class BookService
    {
        public static void SaveFromRawSegments(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            var bookWindow = new BookWindow(segments, activity, unitOfWork);

            if (ConfigurationManager.AppSettings["ShowCategoryWindows"] == "true") {
                bookWindow.ShowDialog();
            }

            var consolidated = new ConsolidatedBookDTO(bookWindow, activity);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        public static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            var consolidated = new ConsolidatedBookDTO(backupSegments);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        private static void SaveFromConsolidated(ConsolidatedBookDTO consolidated, UnitOfWork unitOfWork)
        {
            var series = SeriesService.GetOrCreateByName(consolidated.SeriesName, unitOfWork);
            var book = GetOrUpdateOrCreateBook(consolidated, series, unitOfWork);
            var activity = ActivityService.Create(consolidated, unitOfWork);

            CreateBookActivity(activity, book, consolidated.Description, unitOfWork);
        }

        private static void CreateBookActivity(Activity activity, Book book, string description, UnitOfWork unitOfWork)
        {
            var bookActivity = new BookActivity() {
                Activity = activity,
                Book = book,
                Description = description
            };

            activity.BookActivity = bookActivity;
            ActivityService.PairUpWithStartingActivity(activity, unitOfWork);

            unitOfWork.BookRepo.CreateBookActivity(bookActivity);
        }

        public static List<Book> GetAll(UnitOfWork unitOfWork)
        {
            return unitOfWork.BookRepo.GetAllBooks().ToList();
        }

        public static Book GetOrUpdateOrCreateBook(ConsolidatedBookDTO consolidated, Series series, UnitOfWork unitOfWork)
        {
            var book = GetByTitle(consolidated.Title, unitOfWork);

            if (book == null) {
                book = new Book() {
                    Title = consolidated.Title,
                    Author = consolidated.Author,
                    Series = series,
                    Number = consolidated.Number,
                    Publisher = consolidated.Publisher,
                    Year = int.Parse(consolidated.Year),
                    Score = consolidated.Score,
                };
                unitOfWork.BookRepo.CreateBook(book);
            } else {
                book.Series = series ?? book.Series;
            }

            return book;
        }

        //TODO (add year to search)
        public static Book GetByTitle(string title, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(title)) {
                return null;
            }
            return unitOfWork.BookRepo.GetBookByTitle(title);
        }

        //TODO (add year to search)
        public static IEnumerable<Activity> GetStartingActivities(IQueryable<Activity> previousStartingActivities, Activity activity)
        {
            var book = activity.BookActivity.Book;
            return previousStartingActivities.Where(u =>
                u.CategoryId == ActivityCategory.BOOK_ID
                && u.BookActivity.Book.Title == book.Title
            );
        }
    }
}

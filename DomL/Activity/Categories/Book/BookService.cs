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
using DomL.Business.Utils;

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

            var consolidated = new BookConsolidatedDTO(bookWindow, activity);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        public static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            var consolidated = new BookConsolidatedDTO(backupSegments);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        private static void SaveFromConsolidated(BookConsolidatedDTO consolidated, UnitOfWork unitOfWork)
        {
            var series = SeriesService.GetOrCreateByName(consolidated.Series, unitOfWork);
            var book = GetOrUpdateOrCreateBook(consolidated, series, unitOfWork);
            var activity = ActivityService.Create(consolidated, unitOfWork);

            CreateBookActivity(activity, book, consolidated.Description, unitOfWork);
        }

        private static void CreateBookActivity(Activity activity, Book book, string description, UnitOfWork unitOfWork)
        {
            var bookActivity = new BookActivity() {
                Activity = activity,
                Book = book,
                Description = Util.GetStringOrNull(description)
            };

            activity.BookActivity = bookActivity;
            ActivityService.PairUpWithStartingActivity(activity, unitOfWork);

            unitOfWork.BookRepo.CreateBookActivity(bookActivity);
        }

        public static List<Book> GetAll(UnitOfWork unitOfWork)
        {
            return unitOfWork.BookRepo.GetAllBooks();
        }

        public static Book GetOrUpdateOrCreateBook(BookConsolidatedDTO consolidated, Series series, UnitOfWork unitOfWork)
        {
            var instance = GetByTitle(consolidated.Title, unitOfWork);

            var title = Util.GetStringOrNull(consolidated.Title);
            var number = Util.GetStringOrNull(consolidated.Number);
            var person = Util.GetStringOrNull(consolidated.Person);
            var company = Util.GetStringOrNull(consolidated.Company);
            var year = Util.GetIntOrZero(consolidated.Year);
            var score = Util.GetStringOrNull(consolidated.Score);

            if (instance == null) {
                instance = new Book() {
                    Title = title,
                    Series = series,
                    Number = number,
                    Person = person,
                    Company = company,
                    Year = year,
                    Score = score,
                };
            } else {
                instance.Series = series ?? instance.Series;
                instance.Number = number ?? instance.Number;
                instance.Person = person ?? instance.Person;
                instance.Company = company ?? instance.Company;
                instance.Year = year != 0 ? year : instance.Year;
                instance.Score = score ?? instance.Score;
            }

            return instance;
        }

        //TODO (add year to search)
        public static Book GetByTitle(string title, UnitOfWork unitOfWork)
        {
            if (Util.IsStringEmpty(title)) {
                return null;
            }
            return unitOfWork.BookRepo.GetBookByTitle(title);
        }

        //TODO (add year to search)
        public static IEnumerable<Activity> GetStartingActivities(IQueryable<Activity> previousStartingActivities, Activity activity)
        {
            var book = activity.BookActivity.Book;
            return previousStartingActivities.Where(u =>
                u.CategoryId == Category.BOOK_ID
                && u.BookActivity.Book.Title == book.Title
            );
        }
    }
}

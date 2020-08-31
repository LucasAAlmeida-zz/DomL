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
            // BOOK (Classification); Title; (Author Name); (Series Name); (Number In Series); (Score); (Description)
            var bookWindow = new BookWindow(segments, activity, unitOfWork);

            if (ConfigurationManager.AppSettings["ShowCategoryWindows"] == "true") {
                bookWindow.ShowDialog();
            }

            var bookTitle = bookWindow.TitleCB.Text;
            var authorName = bookWindow.AuthorCB.Text;
            var seriesName = bookWindow.SeriesCB.Text;
            var numberInSeries = (!string.IsNullOrWhiteSpace(bookWindow.NumberCB.Text)) ? bookWindow.NumberCB.Text : null;
            var scoreValue = bookWindow.SeriesCB.Text;
            var description = (!string.IsNullOrWhiteSpace(bookWindow.DescriptionCB.Text)) ? bookWindow.DescriptionCB.Text : null;

            var author = PersonService.GetOrCreateByName(authorName, unitOfWork);
            var series = SeriesService.GetOrCreateByName(seriesName, unitOfWork);
            var score = ScoreService.GetByValue(scoreValue, unitOfWork);

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
            ActivityService.PairUpWithStartingActivity(activity, unitOfWork);

            unitOfWork.BookRepo.CreateBookActivity(bookActivity);
        }

        public static List<Book> GetAll(UnitOfWork unitOfWork)
        {
            return unitOfWork.BookRepo.GetAllBooks().ToList();
        }

        public static Book GetOrUpdateOrCreateBook(string bookTitle, Person author, Series series, string numberInSeries, Score score, UnitOfWork unitOfWork)
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

        public static IEnumerable<Activity> GetStartingActivities(IQueryable<Activity> previousStartingActivities, Activity activity)
        {
            var book = activity.BookActivity.Book;
            return previousStartingActivities.Where(u =>
                u.CategoryId == ActivityCategory.BOOK_ID
                && u.BookActivity.Book.Title == book.Title
            );
        }

        public static void RestoreFromFile(string fileDir)
        {
            using (var reader = new StreamReader(fileDir + "Book.txt")) {
                string line = "";
                while ((line = reader.ReadLine()) != null) {
                    if (string.IsNullOrWhiteSpace(line)) {
                        continue;
                    }

                    var segments = Regex.Split(line, "\t");

                    // Date Start; Date Finish; Title; Author Name; Series Name; Number In Series; Score; Description
                    var dateStart = segments[0];
                    var dateFinish = segments[1];
                    var title = segments[2];
                    var authorName = segments[3] != "-" ? segments[3] : null;
                    var seriesName = segments[4] != "-" ? segments[4] : null;
                    var numberInSeries = segments[5] != "-" ? segments[5] : null;
                    var scoreValue = segments[6] != "-" ? segments[6] : null;
                    var description = segments[7] != "-" ? segments[7] : null;

                    var originalLine = title;
                    originalLine = (!string.IsNullOrWhiteSpace(authorName)) ? originalLine + "; " + authorName : originalLine;
                    originalLine = (!string.IsNullOrWhiteSpace(seriesName)) ? originalLine + "; " + seriesName : originalLine;
                    originalLine = (!string.IsNullOrWhiteSpace(numberInSeries)) ? originalLine + "; " + numberInSeries : originalLine;
                    originalLine = (!string.IsNullOrWhiteSpace(scoreValue)) ? originalLine + "; " + scoreValue : originalLine;
                    originalLine = (!string.IsNullOrWhiteSpace(description)) ? originalLine + "; " + description : originalLine;

                    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                        var author = PersonService.GetOrCreateByName(authorName, unitOfWork);
                        var series = SeriesService.GetOrCreateByName(seriesName, unitOfWork);
                        var score = ScoreService.GetByValue(scoreValue, unitOfWork);

                        Book book = GetOrUpdateOrCreateBook(title, author, series, numberInSeries, score, unitOfWork);

                        var statusStart = unitOfWork.ActivityRepo.GetStatusById(ActivityStatus.START);
                        var statusFinish = unitOfWork.ActivityRepo.GetStatusById(ActivityStatus.FINISH);
                        var statusSingle = unitOfWork.ActivityRepo.GetStatusById(ActivityStatus.SINGLE);

                        var category = unitOfWork.ActivityRepo.GetCategoryById(ActivityCategory.BOOK_ID);

                        if (!dateStart.StartsWith("--") && dateStart != dateFinish) {
                            Activity activityStart = null;
                            Activity activityFinish = null;
                            if (!dateStart.StartsWith("??")) {
                                var date = DateTime.ParseExact(dateStart, "dd/MM/yy", null);
                                activityStart = ActivityService.Create(date, 0, statusStart, category, null, "BOOK Start; " + originalLine, unitOfWork);
                                CreateBookActivity(activityStart, book, description, unitOfWork);
                            }
                            if (!dateFinish.StartsWith("??")) {
                                var date = DateTime.ParseExact(dateFinish, "dd/MM/yy", null);
                                activityFinish = ActivityService.Create(date, 0, statusFinish, category, null, "BOOK Finish; " + originalLine, unitOfWork);
                                CreateBookActivity(activityFinish, book, description, unitOfWork);
                            }

                            if (activityStart != null && activityFinish != null) {
                                activityStart.BookActivity.Description = null;
                                activityStart.PairedActivity = activityFinish;
                                unitOfWork.Complete();
                                activityFinish.PairedActivity = activityStart;
                            }
                        } else {
                            var date = DateTime.ParseExact(dateFinish, "dd/MM/yy", null);
                            var activity = ActivityService.Create(date, 0, statusSingle, category, null, "BOOK; " + originalLine, unitOfWork);
                            CreateBookActivity(activity, book, description, unitOfWork);
                        }

                        unitOfWork.Complete();
                    }
                }
            }
        }
    }
}

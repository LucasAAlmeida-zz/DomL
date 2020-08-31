using DomL.Business.Entities;
using DomL.Business.Utils;

using DomL.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DomL.Business.Services
{
    public class DomLServices
    {
        const string BASE_DIR_PATH = "D:\\OneDrive\\Área de Trabalho\\DomL\\";

        public static void SaveFromRawMonthText(string rawMonthText, int month, int year)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                unitOfWork.ActivityRepo.DeleteAllFromMonth(month, year);
                unitOfWork.Complete();
            }

            ActivityBlock currentActivityBlock = null;
            var date = new DateTime(year, month, 1);
            var dayOrder = 0;

            var activityRawLines = Regex.Split(rawMonthText, "\r\n");
            foreach(var rawLine in activityRawLines) {
                try {
                    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                        if (Util.IsLineBlank(rawLine)) {
                            continue;
                        }

                        if (Util.IsLineNewDay(rawLine, out int dia)) {
                            date = new DateTime(year, month, dia);
                            dayOrder = 0;
                            continue;
                        }

                        if (Util.IsLineActivityBlockTag(rawLine)) {
                            currentActivityBlock = ActivityService.ChangeActivityBlock(rawLine, unitOfWork);
                            continue;
                        }

                        var status = ActivityService.GetStatus(rawLine, unitOfWork);
                        var category = ActivityService.GetCategory(rawLine, unitOfWork);

                        dayOrder++;
                        Activity activity = ActivityService.Create(date, dayOrder, status, category, currentActivityBlock, rawLine, unitOfWork);
                        ActivityService.SaveFromRawLine(activity, rawLine, unitOfWork);

                        unitOfWork.Complete();
                    }
                } catch (Exception e) {
                    var msg = "Deu ruim no dia " + date.Day + ", atividade: " + rawLine;
                    throw new ParseException(msg, e);
                }
            }
        }

        public static void WriteMonthRecapFile(int month, int year)
        {
            string filePath = BASE_DIR_PATH + "MonthRecap\\Month" + month.ToString("00") + "Recap.txt";
            Util.CreateDirectory(filePath);

            List<Activity> monthActivities;
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                monthActivities = unitOfWork.ActivityRepo.GetAllInclusiveFromMonth(month, year);
            }
            
            using (var file = new StreamWriter(filePath)) {
                foreach (Activity activity in monthActivities) {
                    string activityString = ActivityService.GetInfoForMonthRecap(activity);
                    if (!string.IsNullOrWhiteSpace(activityString)) {
                        file.WriteLine(activityString);
                    }
                }
            }
        }

        public static void WriteYearRecapFiles(int year)
        {
            string fileDir = BASE_DIR_PATH + "Year" + year + "Recap\\";
            Util.CreateDirectory(fileDir);

            List<Activity> yearActivities;
            List<ActivityCategory> categories;
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                yearActivities = unitOfWork.ActivityRepo.GetAllInclusiveFromYear(year);
                categories = unitOfWork.ActivityRepo.GetAllCategories();
            }

            foreach(var category in categories) {
                var filePath = fileDir + category.Name + year + ".txt";
                WriteRecapFile(category.Id, filePath, yearActivities);
            }
            WriteStatisticsFile(fileDir, yearActivities);
        }

        public static void WriteRecapFiles()
        {
            string fileDir = BASE_DIR_PATH + "Recap\\";
            Util.CreateDirectory(fileDir);

            List<Activity> activities;
            List<ActivityCategory> categories;
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                activities = unitOfWork.ActivityRepo.GetAllInclusive();
                categories = unitOfWork.ActivityRepo.GetAllCategories();
            }

            foreach (var category in categories) {
                var filePath = fileDir + category.Name + ".txt";
                WriteRecapFile(category.Id, filePath, activities);
            }
            WriteStatisticsFile(fileDir, activities);
        }

        private static void WriteRecapFile(int categoryId, string filePath, List<Activity> activities)
        {
            var categoryActivities = activities.Where(u => u.CategoryId == categoryId).ToList();

            if (categoryActivities.Count == 0) {
                return;
            }

            using (var file = new StreamWriter(filePath)) {
                foreach (var activity in categoryActivities) {
                    string activityString = ActivityService.GetInfoForYearRecap(activity);
                    if (!string.IsNullOrWhiteSpace(activityString)) {
                        file.WriteLine(activityString);
                    }
                }
            }
        }

        private static void WriteStatisticsFile(string fileDir, List<Activity> activities)
        {
            using (var file = new StreamWriter(fileDir + "Statistics.txt")) {
                file.WriteLine("Jogos começados:\t" + CountStarted(activities, ActivityCategory.GAME_ID));
                file.WriteLine("Jogos terminados:\t" + CountFinished(activities, ActivityCategory.GAME_ID));
                file.WriteLine("Temporadas de séries assistidas:\t" + CountFinished(activities, ActivityCategory.SHOW_ID));
                file.WriteLine("Livros lidos:\t" + CountFinished(activities, ActivityCategory.BOOK_ID));
                file.WriteLine("K Páginas de comics lidos:\t" + CountFinished(activities, ActivityCategory.COMIC_ID));
                file.WriteLine("Filmes assistidos:\t" + CountFinished(activities, ActivityCategory.MOVIE_ID));
                file.WriteLine("Viagens feitas:\t" + CountFinished(activities, ActivityCategory.TRAVEL_ID));
                file.WriteLine("Pessoas novas conhecidas:\t" + CountFinished(activities, ActivityCategory.MEET_ID));
                file.WriteLine("Compras notáveis:\t" + CountFinished(activities, ActivityCategory.PURCHASE_ID));
            }
        }

        private static int CountStarted(List<Activity> activities, int categoryId)
        {
            return activities.Count(u => u.CategoryId == categoryId && u.StatusId != ActivityStatus.FINISH);
        }

        private static int CountFinished(List<Activity> activities, int categoryId)
        {
            return activities.Count(u => u.CategoryId == categoryId && u.StatusId != ActivityStatus.START);
        }
    }
}

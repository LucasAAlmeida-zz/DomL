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

        #region SaveFromRawMonthText
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
                    if (IsLineBlank(rawLine)) {
                        continue;
                    }

                    if (IsLineNewDay(rawLine, out int dia)) {
                        date = new DateTime(year, month, dia);
                        dayOrder = 0;
                        continue;
                    }

                    dayOrder++;
                    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                        if (IsLineActivityBlockTag(rawLine)) {
                            currentActivityBlock = ActivityService.ChangeActivityBlock(rawLine, unitOfWork);
                            continue;
                        }
                        Activity activity = ActivityService.Create(date, dayOrder, rawLine, currentActivityBlock, unitOfWork);
                        activity.SaveFromRawLine(rawLine, unitOfWork);

                        unitOfWork.Complete();
                    }

                } catch (Exception e) {
                    var msg = "Deu ruim no dia " + date.Day + ", atividade: " + rawLine;
                    throw new ParseException(msg, e);
                }
            }
        }

        private static bool IsLineActivityBlockTag(string line)
        {
            return line.StartsWith("<");
        }

        private static bool IsLineBlank(string line)
        {
            return string.IsNullOrWhiteSpace(line) || line.StartsWith("---");
        }

        private static bool IsLineNewDay(string linha, out int dia)
        {
            int indexPrimeiroEspaco = linha.IndexOf(" ", StringComparison.Ordinal);
            string firstWord = (indexPrimeiroEspaco != -1) ? linha.Substring(0, indexPrimeiroEspaco) : linha;
            return int.TryParse(firstWord, out dia) && (linha.Contains(" - ") || linha.Contains(" – "));
        }
        #endregion

        #region WriteMonthRecapFile
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
                    string activityString = activity.GetString(0);
                    if (!string.IsNullOrWhiteSpace(activityString)) {
                        file.WriteLine(activityString);
                    }
                }
            }
        }
        #endregion

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
                    string activityString = activity.GetString(1);
                    if (!string.IsNullOrWhiteSpace(activityString)) {
                        file.WriteLine(activityString);
                    }
                }
            }
        }

        private static void WriteStatisticsFile(string fileDir, List<Activity> activities)
        {
            using (var file = new StreamWriter(fileDir + "Statistics.txt")) {

                file.WriteLine("Jogos começados:\t" + CountStarted(activities, ActivityCategory.GAME));
                file.WriteLine("Jogos terminados:\t" + CountFinished(activities, ActivityCategory.GAME));
                file.WriteLine("Temporadas de séries assistidas:\t" + CountFinished(activities, ActivityCategory.SERIES));
                file.WriteLine("Livros lidos:\t" + CountFinished(activities, ActivityCategory.BOOK));
                file.WriteLine("K Páginas de comics lidos:\t" + CountFinished(activities, ActivityCategory.COMIC));
                file.WriteLine("Filmes assistidos:\t" + CountFinished(activities, ActivityCategory.MOVIE));
                file.WriteLine("Viagens feitas:\t" + CountFinished(activities, ActivityCategory.TRAVEL));
                file.WriteLine("Pessoas novas conhecidas:\t" + CountFinished(activities, ActivityCategory.PERSON));
                file.WriteLine("Compras notáveis:\t" + CountFinished(activities, ActivityCategory.PURCHASE));
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

        //private static void WriteRecapFile(int categoryId, string fileDir, List<Activity> yearActivities)
        //{
        //    var activities = yearActivities.Where(u => u.CategoryId == categoryId).ToList();

        //    if (activities.Count == 0) {
        //        return;
        //    }

        //    var categoryName = activities.First().Category.Name;
        //    using (var file = new StreamWriter(fileDir + categoryName + ".txt")) {
        //        foreach (var activity in activities) {
        //            string activityString = activity.GetString(1);
        //            if (!string.IsNullOrWhiteSpace(activityString)) {
        //                file.WriteLine(activityString);
        //            }
        //        }
        //    }
        //}

        //        public static void EscreveResumoDoAnoEmArquivo(int year)
        //        {
        //            string filePath = BASE_DIR_PATH + "ResumoAno.txt";
        //            using (var file = new StreamWriter(filePath)) {
        //                file.WriteLine("Jogos começados:\t" + Game.CountBegunYear(year));
        //                file.WriteLine("Jogos terminados:\t" + Game.CountEndedYear(year));
        //                file.WriteLine("Temporadas de séries assistidas:\t" + Series.CountEndedYear(year));
        //                file.WriteLine("Livros lidos:\t" + Book.CountEndedYear(year));
        //                file.WriteLine("K Páginas de comics lidos:\t" + Comic.CountEndedYear(year));
        //                file.WriteLine("Filmes assistidos:\t" + Movie.CountYear(year));
        //                file.WriteLine("Viagens feitas:\t" + Travel.CountYear(year));
        //                file.WriteLine("Pessoas novas conhecidas:\t" + Person.CountYear(year));
        //                file.WriteLine("Compras notáveis:\t" + Purchase.CountYear(year));
        //            }
        //        }

        //        public static void EscreverAtividadesConsolidadasOfAllTimeEmArquivo()
        //        {
        //            string fileDir = BASE_DIR_PATH + "AtividadesConsolidadas\\";
        //            var fi = new FileInfo(fileDir);
        //            if (fi.Directory != null && !fi.Directory.Exists && fi.DirectoryName != null) {
        //                Directory.CreateDirectory(fi.DirectoryName);
        //            }

        //            Book.ConsolidateAll(fileDir);
        //            Comic.ConsolidateAll(fileDir);
        //            Game.ConsolidateAll(fileDir);
        //            Series.ConsolidateAll(fileDir);
        //            Watch.ConsolidateAll(fileDir);

        //            Auto.ConsolidateAll(fileDir);
        //            Doom.ConsolidateAll(fileDir);
        //            Gift.ConsolidateAll(fileDir);
        //            Health.ConsolidateAll(fileDir);
        //            Movie.ConsolidateAll(fileDir);
        //            Person.ConsolidateAll(fileDir);
        //            Pet.ConsolidateAll(fileDir);
        //            Play.ConsolidateAll(fileDir);
        //            Purchase.ConsolidateAll(fileDir);
        //            Travel.ConsolidateAll(fileDir);
        //            Work.ConsolidateAll(fileDir);
        //        }

        //        public static void RestoreDatabaseFromFile()
        //        {
        //            string fileDir = BASE_DIR_PATH + "AtividadesConsolidadas\\";

        //            Auto.FullRestoreFromFile(fileDir);
        //            Doom.FullRestoreFromFile(fileDir);
        //            Gift.FullRestoreFromFile(fileDir);
        //            Health.FullRestoreFromFile(fileDir);
        //            Movie.FullRestoreFromFile(fileDir);
        //            Person.FullRestoreFromFile(fileDir);
        //            Pet.FullRestoreFromFile(fileDir);
        //            Play.FullRestoreFromFile(fileDir);
        //            Purchase.FullRestoreFromFile(fileDir);
        //            Travel.FullRestoreFromFile(fileDir);
        //            Work.FullRestoreFromFile(fileDir);
        //        }

        //        public static void RestoreBooksFromFile()
        //        {
        //            Book.FullRestoreFromFile(BASE_DIR_PATH + "Backup\\");
        //        }
        //        public static void RestoreComicsFromFile()
        //        {
        //            Comic.FullRestoreFromFile(BASE_DIR_PATH + "Backup\\");
        //        }
        //        public static void RestoreGamesFromFile()
        //        {
        //            Game.FullRestoreFromFile(BASE_DIR_PATH + "Backup\\");
        //        }
        //        public static void RestoreSeriesFromFile()
        //        {
        //            Series.FullRestoreFromFile(BASE_DIR_PATH + "Backup\\");
        //        }
        //        public static void RestoreWatchsFromFile()
        //        {
        //            Watch.FullRestoreFromFile(BASE_DIR_PATH + "Backup\\");
        //        }
    }
}

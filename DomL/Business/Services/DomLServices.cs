using DomL.Business.Entities;
using DomL.Business.Utils;

using DomL.Business.Utils.Enums;
using DomL.DataAccess;
using DomL.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DomL.Business.Services
{
    public class DomLServices
    {
        //const string BASE_DIR_PATH = "D:\\OneDrive\\Área de Trabalho\\DomL\\";

        public static void ParseAtividadesDoMesEmTextoParaBanco(string atividadesStr, int mes, int ano)
        {
            var activityRepo = new ActivityRepository(new DomLContext());

            var atividadesDiaString = Regex.Split(atividadesStr, "\r\n");
            var atividadeString = "";
            int? activityBlockId = null;
            var ordem = 0;

            var diaDT = new DateTime();
            try {
                for (var linha = 0; linha < atividadesDiaString.Length; linha++) {
                    atividadeString = atividadesDiaString[linha];
                    ordem++;

                    if (IsLineBlank(atividadeString)) {
                        continue;
                    }

                    if (IsLineNewDay(atividadeString, out int dia)) {
                        diaDT = new DateTime(ano, mes, dia);
                        ordem = 0;
                        continue;
                    }

                    if (IsLineActivityBlockTag(atividadeString)) {
                        activityBlockId = GetNewActivityBlockId(atividadeString, activityRepo);
                    }

                    var segmentos = Regex.Split(atividadeString, "; ");

                    var classificacao = GetClassificacaoFromFirstSegment(segmentos[0]);
                    var activity = new Activity() {
                        Date = diaDT,
                        DayOrder = ordem,
                        ActivityBlockId = activityBlockId,
                        Classification = classificacao
                    };

                    var categoria = GetCategoriaFromFirstSegment(segmentos[0]);
                    ParseActivityCategory(categoria, segmentos, activity);
                    activityRepo.CreateActivity(activity);
                }
            } catch (Exception e) {
                var msg = "Deu ruim no dia " + diaDT.Day + ", atividade: " + atividadeString;
                throw new ParseException(msg, e);
            }
        }

        private static void ParseActivityCategory(string categoria, string[] segmentos, Activity activity)
        {
            switch (categoria) {
                case "AUTO":
                case "CARRO":
                    activity.Category = Category.Auto;
                    AutoService.SaveFromRawLine(segmentos, activity);
                    break;

                //case "DOOM":
                //case "DESGRACA":
                //case "DESGRAÇA":
                //    activity.Category = Category.Doom;
                //    new Doom(atividadeDTO, segmentos).Save();
                //    break;

                //case "GIFT":
                //case "PRESENTE":
                //    new Gift(atividadeDTO, segmentos).Save();
                //    break;

                //case "SAUDE":
                //case "HEALTH":
                //    new Health(atividadeDTO, segmentos).Save();
                //    break;

                //case "FILME":
                //case "MOVIE":
                //case "CINEMA":
                //    new Movie(atividadeDTO, segmentos).Save();
                //    break;

                //case "PESSOA":
                //    new Person(atividadeDTO, segmentos).Save();
                //    break;

                //case "PET":
                //case "ANIMAL":
                //    new Pet(atividadeDTO, segmentos).Save();
                //    break;

                //case "PLAY":
                //    new Play(atividadeDTO, segmentos).Save();
                //    break;

                //case "COMPRA":
                //    new Purchase(atividadeDTO, segmentos).Save();
                //    break;

                //case "VIAGEM":
                //case "TRIP":
                //    new Travel(atividadeDTO, segmentos).Save();
                //    break;

                //case "WORK":
                //    new Work(atividadeDTO, segmentos).Save();
                //    break;

                //// -----

                case "LIVRO":
                case "BOOK":
                    activity.Category = Category.Book;
                    BookService.SaveFromRawLine(segmentos, activity);
                    break;

                    //case "COMIC":
                    //case "MANGA":
                    //    new Comic(atividadeDTO, segmentos).Save();
                    //    break;

                    //case "JOGO":
                    //case "GAME":
                    //    new Game(atividadeDTO, segmentos).Save();
                    //    break;

                    //case "SERIE":
                    //case "DESENHO":
                    //case "ANIME":
                    //case "CARTOON":
                    //    new Series(atividadeDTO, segmentos).Save();
                    //    break;

                    //case "WATCH":
                    //    new Watch(atividadeDTO, segmentos).Save();
                    //    break;

                    //default:
                    //    new Event(atividadeDTO, segmentos).Save();
                    //    break;
            }
        }

        private static string GetCategoriaFromFirstSegment(string firstSegment)
        {
            return Regex.Split(firstSegment, " ")[0];
        }

        private static Classification GetClassificacaoFromFirstSegment(string firstSegment)
        {
            var segments = Regex.Split(firstSegment, " ");

            if (segments.Length == 1) {
                return Classification.Unica;
            }
 
            return IsComeco(segments[1]) ? Classification.Comeco : Classification.Termino;
        }

        private static int? GetNewActivityBlockId(string atividadeString, ActivityRepository activityRepo)
        {
            if (atividadeString == "<END>") {
                return null;
            } 
            
            var activityBlockName = atividadeString.Substring(1, atividadeString.Length - 2);
            var activityBlock = new ActivityBlock() {
                Name = activityBlockName
            };
            return activityRepo.CreateActivityBlock(activityBlock).Id;
        }

        private static bool IsLineActivityBlockTag(string line)
        {
            return line.StartsWith("<");
        }

        private static bool IsLineBlank(string line)
        {
            return string.IsNullOrWhiteSpace(line) || line.StartsWith("---");
        }

        private static bool IsComeco(string word)
        {
            return word.ToLower() == "comeco" || word.ToLower() == "começo";
        }

        private static bool IsLineNewDay(string linha, out int dia)
        {
            int indexPrimeiroEspaco = linha.IndexOf(" ", StringComparison.Ordinal);
            string firstWord = (indexPrimeiroEspaco != -1) ? linha.Substring(0, indexPrimeiroEspaco) : linha;
            return int.TryParse(firstWord, out dia) && (linha.Contains(" - ") || linha.Contains(" – "));
        }

        //        public static void EscreverAtividadesDoMesEmArquivo(int mes, int ano)
        //        {
        //            string filePath = BASE_DIR_PATH + "AtividadesMes\\AtividadesMes" + mes + ".txt";
        //            var fi = new FileInfo(filePath);
        //            if (fi.Directory != null && !fi.Directory.Exists && fi.DirectoryName != null) {
        //                Directory.CreateDirectory(fi.DirectoryName);
        //            }

        //            var atividades = GetAllAtividadesMes(mes, ano);
        //            using (var file = new StreamWriter(filePath)) {
        //                foreach (Activity atividade in atividades) {
        //                    file.WriteLine(atividade.ParseToString());
        //                }
        //            }
        //        }

        //        private static List<Activity> GetAllAtividadesMes(int mes, int ano)
        //        {
        //            var atividades = new List<Activity>();

        //            atividades.AddRange(Book.GetAllFromMes(mes, ano));
        //            atividades.AddRange(Comic.GetAllFromMes(mes, ano));
        //            atividades.AddRange(Game.GetAllFromMes(mes, ano));
        //            atividades.AddRange(Series.GetAllFromMes(mes, ano));
        //            atividades.AddRange(Watch.GetAllFromMes(mes, ano));

        //            atividades.AddRange(Auto.GetAllFromMes(mes, ano));
        //            atividades.AddRange(Doom.GetAllFromMes(mes, ano));
        //            atividades.AddRange(Gift.GetAllFromMes(mes, ano));
        //            atividades.AddRange(Health.GetAllFromMes(mes, ano));
        //            atividades.AddRange(Movie.GetAllFromMes(mes, ano));
        //            atividades.AddRange(Person.GetAllFromMes(mes, ano));
        //            atividades.AddRange(Pet.GetAllFromMes(mes, ano));
        //            atividades.AddRange(Play.GetAllFromMes(mes, ano));
        //            atividades.AddRange(Purchase.GetAllFromMes(mes, ano));
        //            atividades.AddRange(Travel.GetAllFromMes(mes, ano));
        //            atividades.AddRange(Work.GetAllFromMes(mes, ano));

        //            atividades.AddRange(Event.GetImportantFromMes(mes, ano));

        //            return atividades.OrderBy(a => a.Date).ThenBy(a => a.DayOrder).ToList();
        //        }

        //        public static void EscreverAtividadesConsolidadasDoAnoEmArquivo(int year)
        //        {
        //            string fileDir = BASE_DIR_PATH + "AtividadesConsolidadas" + year + "\\";
        //            var fi = new FileInfo(fileDir);
        //            if (fi.Directory != null && !fi.Directory.Exists && fi.DirectoryName != null) {
        //                Directory.CreateDirectory(fi.DirectoryName);
        //            }

        //            Book.ConsolidateYear(fileDir, year);
        //            Comic.ConsolidateYear(fileDir, year);
        //            Game.ConsolidateYear(fileDir, year);
        //            Series.ConsolidateYear(fileDir, year);
        //            Watch.ConsolidateYear(fileDir, year);

        //            Auto.ConsolidateYear(fileDir, year);
        //            Doom.ConsolidateYear(fileDir, year);
        //            Gift.ConsolidateYear(fileDir, year);
        //            Health.ConsolidateYear(fileDir, year);
        //            Movie.ConsolidateYear(fileDir, year);
        //            Person.ConsolidateYear(fileDir, year);
        //            Pet.ConsolidateYear(fileDir, year);
        //            Play.ConsolidateYear(fileDir, year);
        //            Purchase.ConsolidateYear(fileDir, year);
        //            Travel.ConsolidateYear(fileDir, year);
        //            Work.ConsolidateYear(fileDir, year);

        //            Event.ConsolidateYear(fileDir, year);

        //            ActivityBlock.ConsolidateYear(fileDir, year);
        //        }

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

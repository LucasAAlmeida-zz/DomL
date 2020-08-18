//using DomL.Business.Utils;
//using DomL.Business.Utils.DTOs;
//using DomL.Business.Utils.Enums;
//using DomL.DataAccess;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.IO;
//using System.Linq;
//using System.Text.RegularExpressions;

//namespace DomL.Business.Activities.MultipleDayActivities
//{
//    [Table("Comic")]
//    public class Comic : MultipleDayActivity
//    {
//        public Comic(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }
//        public Comic() { }

//        // COMIC|MANGA; (De Quem) Autor; (Assunto) Título; (Classificação) Término; (Valor) Nota; (Descrição) O que achei

//        public override void Save()
//        {
//            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
//                if (unitOfWork.ComicRepo.Exists(b => b.Date == this.Date && b.Subject == this.Subject)) {
//                    return;
//                }

//                unitOfWork.ComicRepo.Add(this);
//                unitOfWork.Complete();
//            }
//        }

//        public static IEnumerable<Comic> GetAllFromMes(int mes, int ano)
//        {
//            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
//                return unitOfWork.ComicRepo.Find(b => b.Date.Month == mes && b.Date.Year == ano);
//            }
//        }

//        public static void ConsolidateYear(string fileDir, int year)
//        {
//            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
//                var allComics = unitOfWork.ComicRepo.Find(b => b.Date.Year == year).ToList();
//                EscreveConsolidadasNoArquivo(fileDir + "Comic" + year + ".txt", allComics.Cast<MultipleDayActivity>().ToList());
//            }
//        }

//        public static void ConsolidateAll(string fileDir)
//        {
//            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
//                var allComics = unitOfWork.ComicRepo.GetAll().ToList();
//                EscreveConsolidadasNoArquivo(fileDir + "Comic.txt", allComics.Cast<MultipleDayActivity>().ToList());
//            }
//        }

//        public static int CountEndedYear(int ano)
//        {
//            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
//                return unitOfWork.ComicRepo
//                    .Find(g =>
//                        (g.Classificacao == Classification.Termino || g.Classificacao == Classification.Unica)
//                        && g.Date.Year == ano)
//                    .Count();
//            }
//        }

//        public static void FullRestoreFromFile(string fileDir)
//        {
//            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
//                var allComics = GetComicsFromFile(fileDir + "Comic.txt");
//                unitOfWork.ComicRepo.AddRange(allComics);
//                unitOfWork.Complete();
//            }
//        }

//        private static List<Comic> GetComicsFromFile(string filePath)
//        {
//            var comics = new List<Comic>();
//            using (var reader = new StreamReader(filePath)) {

//                string line = "";
//                try {
//                    while ((line = reader.ReadLine()) != null) {
//                        if (string.IsNullOrWhiteSpace(line)) {
//                            continue;
//                        }
//                        var segmentos = Regex.Split(line, "\t");

//                        // DataInicio; DataFim; (De Quem); (Assunto); (Nota); (Descrição)

//                        int? nota = segmentos[4] != "-" ? int.Parse(segmentos[4]) : (int?) null;
//                        string descricao = segmentos[5] != "-" ? segmentos[5] : null;

//                        if (segmentos[0] == segmentos[1]) {
//                            var comic = new Comic() {
//                                Date = DateTime.ParseExact(segmentos[0], "dd/MM/yy", null),
//                                Classificacao = Classification.Unica,
//                                DeQuem = segmentos[2],
//                                Subject = segmentos[3],
//                                Nota = nota,
//                                Description = descricao,

//                                DayOrder = 0,
//                            };
//                            comics.Add(comic);
//                            continue;
//                        }

//                        if (!segmentos[0].StartsWith("??/??")) {
//                            var comic = new Comic() {
//                                Date = DateTime.ParseExact(segmentos[0], "dd/MM/yy", null),
//                                Classificacao = Classification.Comeco,
//                                DeQuem = segmentos[2],
//                                Subject = segmentos[3],
//                                Nota = nota,
//                                Description = segmentos[1].StartsWith("??/??") ? descricao : null,

//                                DayOrder = 0,
//                            };
//                            comics.Add(comic);
//                        }

//                        if (!segmentos[1].StartsWith("??/??")) {
//                            var comic = new Comic() {
//                                Date = DateTime.ParseExact(segmentos[1], "dd/MM/yy", null),
//                                Classificacao = Classification.Termino,
//                                DeQuem = segmentos[2],
//                                Subject = segmentos[3],
//                                Nota = nota,
//                                Description = descricao,

//                                DayOrder = 0,
//                            };
//                            comics.Add(comic);
//                        }
//                    }
//                } catch (Exception e) {
//                    var msg = "Deu ruim na linha " + line;
//                    throw new ParseException(msg, e);
//                }
//            }
//            return comics;
//        }
//    }
//}

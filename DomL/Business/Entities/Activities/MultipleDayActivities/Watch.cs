using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using DomL.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DomL.Business.Activities.MultipleDayActivities
{
    [Table("Watch")]
    public class Watch : MultipleDayActivity
    {
        public Watch(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }
        public Watch() { }

        // WATCH; (De Quem) Plataforma; (Assunto) Título; (Classificação) Término; (Valor) Nota; (Descrição) O que achei

        public override void Save()
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                if (unitOfWork.WatchRepo.Exists(b => b.Date == this.Date && b.Subject == this.Subject)) {
                    return;
                }

                unitOfWork.WatchRepo.Add(this);
                unitOfWork.Complete();
            }
        }

        public static IEnumerable<Watch> GetAllFromMes(int mes, int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.WatchRepo.Find(b => b.Date.Month == mes && b.Date.Year == ano);
            }
        }

        public static void ConsolidateYear(string fileDir, int year)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allWatch = unitOfWork.WatchRepo.Find(b => b.Date.Year == year).ToList();
                EscreveConsolidadasNoArquivo(fileDir + "Watch" + year + ".txt", allWatch.Cast<MultipleDayActivity>().ToList());
            }
        }

        public static void ConsolidateAll(string fileDir)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allWatch = unitOfWork.WatchRepo.GetAll().ToList();
                EscreveConsolidadasNoArquivo(fileDir + "Watch.txt", allWatch.Cast<MultipleDayActivity>().ToList());
            }
        }

        public static int CountEndedYear(int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.WatchRepo
                    .Find(g =>
                        (g.Classificacao == Classification.Termino || g.Classificacao == Classification.Unica)
                        && g.Date.Year == ano)
                    .Count();
            }
        }

        public static void FullRestoreFromFile(string fileDir)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allWatchs = GetWatchsFromFile(fileDir + "Watch.txt");
                unitOfWork.WatchRepo.AddRange(allWatchs);
                unitOfWork.Complete();
            }
        }

        private static List<Watch> GetWatchsFromFile(string filePath)
        {
            if (!File.Exists(filePath)) {
                return null;
            }

            var watchs = new List<Watch>();
            using (var reader = new StreamReader(filePath)) {

                string line;
                while ((line = reader.ReadLine()) != null) {
                    var segmentos = Regex.Split(line, "\t");

                    // DataInicio; DataFim; (De Quem); (Assunto); (Nota); (Descrição)

                    if (segmentos[0] == segmentos[1]) {
                        var watch = new Watch() {
                            Date = DateTime.Parse(segmentos[0]),
                            Classificacao = Classification.Unica,
                            DeQuem = segmentos[2],
                            Subject = segmentos[3],
                            Nota = int.Parse(segmentos[4]),
                            Description = segmentos[5],

                            DayOrder = 0,
                        };
                        watchs.Add(watch);
                        continue;
                    }

                    if (!segmentos[0].StartsWith("??/??")) {
                        var watch = new Watch() {
                            Date = DateTime.Parse(segmentos[0]),
                            Classificacao = Classification.Comeco,
                            DeQuem = segmentos[2],
                            Subject = segmentos[3],
                            Nota = int.Parse(segmentos[4]),
                            Description = segmentos[5],

                            DayOrder = 0,
                        };
                        watchs.Add(watch);
                    }

                    if (!segmentos[1].StartsWith("??/??")) {
                        var watch = new Watch() {
                            Date = DateTime.Parse(segmentos[1]),
                            Classificacao = Classification.Termino,
                            DeQuem = segmentos[2],
                            Subject = segmentos[3],
                            Nota = int.Parse(segmentos[4]),
                            Description = segmentos[5],

                            DayOrder = 0,
                        };
                        watchs.Add(watch);
                    }
                }
            }
            return watchs;
        }
    }
}

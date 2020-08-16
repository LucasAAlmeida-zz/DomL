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
    [Table("Series")]
    public class Series : MultipleDayActivity
    {
        public Series(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }
        public Series() { }

        // SERIE; (De Quem) Produtora; (Assunto) Título; (Classificação) Término; (Valor) Nota; (Descrição) O que achei

        public override void Save()
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                if (unitOfWork.SeriesRepo.Exists(b => b.Date == this.Date && b.Subject == this.Subject)) {
                    return;
                }

                unitOfWork.SeriesRepo.Add(this);
                unitOfWork.Complete();
            }
        }

        public static IEnumerable<Series> GetAllFromMes(int mes, int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.SeriesRepo.Find(b => b.Date.Month == mes && b.Date.Year == ano);
            }
        }

        public static void ConsolidateYear(string fileDir, int year)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allSeries = unitOfWork.SeriesRepo.Find(b => b.Date.Year == year).ToList();
                EscreveConsolidadasNoArquivo(fileDir + "Series" + year + ".txt", allSeries.Cast<MultipleDayActivity>().ToList());
            }
        }

        public static void ConsolidateAll(string fileDir)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allSeries = unitOfWork.SeriesRepo.GetAll().ToList();
                EscreveConsolidadasNoArquivo(fileDir + "Series.txt", allSeries.Cast<MultipleDayActivity>().ToList());
            }
        }

        public static int CountEndedYear(int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.SeriesRepo
                    .Find(g =>
                        (g.Classificacao == Classification.Termino || g.Classificacao == Classification.Unica)
                        && g.Date.Year == ano)
                    .Count();
            }
        }

        public static void FullRestoreFromFile(string fileDir)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allSeriess = GetSeriessFromFile(fileDir + "Series.txt");
                unitOfWork.SeriesRepo.AddRange(allSeriess);
                unitOfWork.Complete();
            }
        }

        private static List<Series> GetSeriessFromFile(string filePath)
        {
            if (!File.Exists(filePath)) {
                return null;
            }

            var seriess = new List<Series>();
            using (var reader = new StreamReader(filePath)) {

                string line;
                while ((line = reader.ReadLine()) != null) {
                    var segmentos = Regex.Split(line, "\t");

                    // DataInicio; DataFim; (De Quem); (Assunto); (Nota); (Descrição)

                    if (segmentos[0] == segmentos[1]) {
                        var series = new Series() {
                            Date = DateTime.Parse(segmentos[0]),
                            Classificacao = Classification.Unica,
                            DeQuem = segmentos[2],
                            Subject = segmentos[3],
                            Nota = int.Parse(segmentos[4]),
                            Description = segmentos[5],

                            DayOrder = 0,
                        };
                        seriess.Add(series);
                        continue;
                    }

                    if (!segmentos[0].StartsWith("??/??")) {
                        var series = new Series() {
                            Date = DateTime.Parse(segmentos[0]),
                            Classificacao = Classification.Comeco,
                            DeQuem = segmentos[2],
                            Subject = segmentos[3],
                            Nota = int.Parse(segmentos[4]),
                            Description = segmentos[5],

                            DayOrder = 0,
                        };
                        seriess.Add(series);
                    }

                    if (!segmentos[1].StartsWith("??/??")) {
                        var series = new Series() {
                            Date = DateTime.Parse(segmentos[1]),
                            Classificacao = Classification.Termino,
                            DeQuem = segmentos[2],
                            Subject = segmentos[3],
                            Nota = int.Parse(segmentos[4]),
                            Description = segmentos[5],

                            DayOrder = 0,
                        };
                        seriess.Add(series);
                    }
                }
            }
            return seriess;
        }
    }
}

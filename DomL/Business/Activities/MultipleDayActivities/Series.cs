using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using DomL.DataAccess;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DomL.Business.Activities.MultipleDayActivities
{
    [Table("Series")]
    public class Series : MultipleDayActivity
    {
        public Series(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }

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

        public static IEnumerable<Series> GetAllFromAno(int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.SeriesRepo.Find(b => b.Date.Year == ano);
            }
        }
        public static void Consolidate(string fileDir, int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allSeries = unitOfWork.SeriesRepo.Find(b => b.Date.Year == ano).ToList();
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
    }
}

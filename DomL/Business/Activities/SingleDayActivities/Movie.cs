using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.DataAccess;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DomL.Business.Activities.SingleDayActivities
{
    [Table("Movie")]
    public class Movie : SingleDayActivity
    {
        [Required]
        public string Nota { get; set; }

        public Movie(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }

        protected override void PopulateActivity(IReadOnlyList<string> segmentos)
        {
            // FILME; (Assunto) Título; (Nota)
            // FILME; (Assunto) Título; (Nota); (Descrição) O que achei

            this.Subject = segmentos[1];
            this.Nota = segmentos[2];
            if (segmentos.Count == 4) {
                this.Description = segmentos[3];
            }
        }

        public override void Save()
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                if (unitOfWork.MovieRepo.Exists(b => b.Date == this.Date)) {
                    return;
                }

                unitOfWork.MovieRepo.Add(this);
                unitOfWork.Complete();
            }
        }

        public static IEnumerable<Movie> GetAllFromMes(int mes, int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.MovieRepo.Find(b => b.Date.Month == mes && b.Date.Year == ano);
            }
        }

        public static IEnumerable<Movie> GetAllFromAno(int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.MovieRepo.Find(b => b.Date.Year == ano);
            }
        }

        public override string ParseToString()
        {
            return Util.GetDiaMes(this.Date) + "\t" + this.Subject + "\t" + this.Nota + "\t" + this.Description;
        }

        public static void Consolidate(string fileDir, int year)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allMovie = unitOfWork.MovieRepo.Find(b => b.Date.Year == year).ToList();
                EscreveConsolidadasNoArquivo(fileDir + "Movie" + year + ".txt", allMovie.Cast<SingleDayActivity>().ToList());
            }
        }

        public static int CountYear(int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.MovieRepo.Find(g => g.Date.Year == ano).Count();
            }
        }
    }
}

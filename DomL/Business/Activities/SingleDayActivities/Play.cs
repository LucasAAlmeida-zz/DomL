using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.DataAccess;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DomL.Business.Activities.SingleDayActivities
{
    [Table("Play")]
    public class Play : SingleDayActivity
    {
        public Play(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }

        protected override void PopulateActivity(IReadOnlyList<string> segmentos)
        {
            //PLAY; (Assunto) Pessoa; (Descricao) O que Aconteceu

            this.Subject = segmentos[1];
            this.Description = segmentos[2];
        }

        public override void Save()
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                if (unitOfWork.PlayRepo.Exists(b => b.Date == this.Date)) {
                    return;
                }

                unitOfWork.PlayRepo.Add(this);
                unitOfWork.Complete();
            }
        }

        public static IEnumerable<Play> GetAllFromMes(int mes, int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.PlayRepo.Find(b => b.Date.Month == mes && b.Date.Year == ano);
            }
        }

        public static IEnumerable<Play> GetAllFromAno(int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.PlayRepo.Find(b => b.Date.Year == ano);
            }
        }

        public override string ParseToString()
        {
            return Util.GetDiaMes(this.Date) + "\t" + this.Subject + "\t" + this.Description;
        }

        public static void Consolidate(string fileDir, int year)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allPlay = unitOfWork.PlayRepo.Find(b => b.Date.Year == year).ToList();
                EscreveConsolidadasNoArquivo(fileDir + "Play" + year + ".txt", allPlay.Cast<SingleDayActivity>().ToList());
            }
        }
    }
}

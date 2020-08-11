using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.DataAccess;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DomL.Business.Activities.SingleDayActivities
{
    [Table("Work")]
    public class Work : SingleDayActivity
    {
        public Work(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }

        protected override void PopulateActivity(IReadOnlyList<string> segmentos)
        {
            //WORK; (Descrição) O que aconteceu

            this.Description = segmentos[1];
        }

        public override void Save()
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                if (unitOfWork.WorkRepo.Exists(b => b.Date == this.Date)) {
                    return;
                }

                unitOfWork.WorkRepo.Add(this);
                unitOfWork.Complete();
            }
        }

        public static IEnumerable<Work> GetAllFromMes(int mes, int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.WorkRepo.Find(b =>b.Date.Month == mes && b.Date.Year == ano);
            }
        }

        public static IEnumerable<Work> GetAllFromAno(int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.WorkRepo.Find(b => b.Date.Year == ano);
            }
        }

        public override string ParseToString()
        {
            return Util.GetDiaMes(this.Date) + "\t" + this.Description;
        }

        public static void Consolidate(string fileDir, int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allWork = unitOfWork.WorkRepo.Find(b => b.Date.Year == ano).ToList();
                EscreveConsolidadasNoArquivo(fileDir + "Work" + ano + ".txt", allWork.Cast<SingleDayActivity>().ToList());
            }
        }
    }
}

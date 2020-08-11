using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.DataAccess;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DomL.Business.Activities.SingleDayActivities
{
    [Table("Auto")]
    public class Auto : SingleDayActivity
    {
        public Auto(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }

        protected override void PopulateActivity(IReadOnlyList<string> segmentos)
        {
            //AUTO; (Assunto) Qual automovel; (Descricao) O que Aconteceu

            this.Subject = segmentos[1];
            this.Description = segmentos[2];
        }

        public override void Save()
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                if (unitOfWork.AutoRepo.Exists(b => b.Date == this.Date)) {
                    return;
                }

                unitOfWork.AutoRepo.Add(this);
                unitOfWork.Complete();
            }
        }

        public override string ParseToString()
        {
            return Util.GetDiaMes(this.Date) + "\t" + this.Subject + "\t" + this.Description;
        }

        public static IEnumerable<Auto> GetAllFromMes(int mes, int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.AutoRepo.Find(b => b.Date.Month == mes && b.Date.Year == ano);
            }
        }

        public static IEnumerable<Auto> GetAllFromAno(int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.AutoRepo.Find(b => b.Date.Year == ano);
            }
        }

        public static void Consolidate(string fileDir, int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allAutos = unitOfWork.AutoRepo.Find(b => b.Date.Year == ano).ToList();
                EscreveConsolidadasNoArquivo(fileDir + "Auto.txt", allAutos.Cast<SingleDayActivity>().ToList());
            }
        }
    }
}

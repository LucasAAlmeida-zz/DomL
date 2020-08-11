using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.DataAccess;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DomL.Business.Activities.SingleDayActivities
{
    [Table("Health")]
    public class Health : SingleDayActivity
    {
        public Health(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }

        protected override void PopulateActivity(IReadOnlyList<string> segmentos)
        {
            //SAUDE; (Descrição) o que aconteceu
            //SAUDE; (Assunto) Especialidade médica; (Descrição) o que aconteceu

            if (segmentos.Count == 2) {
                this.Description = segmentos[1];
            } else {
                this.Subject = segmentos[1];
                this.Description = segmentos[2];
            }
        }

        public override void Save()
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                if (unitOfWork.HealthRepo.Exists(b => b.Date == this.Date)) {
                    return;
                }

                unitOfWork.HealthRepo.Add(this);
                unitOfWork.Complete();
            }
        }

        public static IEnumerable<Health> GetAllFromMes(int mes, int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.HealthRepo.Find(b => b.Date.Month == mes && b.Date.Year == ano);
            }
        }

        public static IEnumerable<Health> GetAllFromAno(int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.HealthRepo.Find(b => b.Date.Year == ano);
            }
        }

        public override string ParseToString()
        {
            string assunto = !string.IsNullOrWhiteSpace(this.Subject) ? this.Subject : "-";
            return Util.GetDiaMes(this.Date) + "\t" + assunto + "\t" + this.Description;
        }

        public static void Consolidate(string fileDir, int year)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allHealth = unitOfWork.HealthRepo.Find(b => b.Date.Year == year).ToList();
                EscreveConsolidadasNoArquivo(fileDir + "Health" + year + ".txt", allHealth.Cast<SingleDayActivity>().ToList());
            }
        }
    }
}

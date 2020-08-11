using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.DataAccess;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DomL.Business.Activities.SingleDayActivities
{
    [Table("Travel")]
    public class Travel : SingleDayActivity
    {
        [Required]
        public string MeioTransporte { get; set; }

        public Travel(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }

        protected override void PopulateActivity(IReadOnlyList<string> segmentos)
        {
            //VIAGEM; (Assunto) De onde pra onde; (MeioTransporte)
            //VIAGEM; (Assunto) De onde pra onde; (MeioTransporte); (Descrição) o que aconteceu

            this.Subject = segmentos[1];
            this.MeioTransporte = segmentos[2];
            if (segmentos.Count == 4)
            {
                this.Description = segmentos[3];
            }
        }

        public static IEnumerable<Travel> GetAllFromMes(int mes, int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.TravelRepo.Find(b => b.Date.Month == mes && b.Date.Year == ano);
            }
        }

        public static IEnumerable<Travel> GetAllFromAno(int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.TravelRepo.Find(b => b.Date.Year == ano);
            }
        }

        public override void Save()
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                if (unitOfWork.TravelRepo.Exists(b => b.Date == this.Date)) {
                    return;
                }

                unitOfWork.TravelRepo.Add(this);
                unitOfWork.Complete();
            }
        }

        public override string ParseToString()
        {
            return Util.GetDiaMes(this.Date) + "\t" + this.Subject + "\t" + this.MeioTransporte + "\t" + this.Description;
        }

        public static void Consolidate(string fileDir, int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allTravel = unitOfWork.TravelRepo.Find(b => b.Date.Year == ano).ToList();
                EscreveConsolidadasNoArquivo(fileDir + "Travel" + ano + ".txt", allTravel.Cast<SingleDayActivity>().ToList());
            }
        }

        public static int CountYear(int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.TravelRepo.Find(g => g.Date.Year == ano).Count();
            }
        }
    }
}

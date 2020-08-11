using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.DataAccess;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DomL.Business.Activities.SingleDayActivities
{
    [Table("Pet")]
    public class Pet : SingleDayActivity
    {
        public Pet(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }

        protected override void PopulateActivity(IReadOnlyList<string> segmentos)
        {
            //PET; (Assunto) Qual Pet; (Descricao) O que Aconteceu

            this.Subject = segmentos[1];
            this.Description = segmentos[2];
        }

        public override void Save()
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                if (unitOfWork.PetRepo.Exists(b => b.Date == this.Date)) {
                    return;
                }

                unitOfWork.PetRepo.Add(this);
                unitOfWork.Complete();
            }
        }

        public static IEnumerable<Pet> GetAllFromMes(int mes, int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.PetRepo.Find(b => b.Date.Month == mes && b.Date.Year == ano);
            }
        }

        public static IEnumerable<Pet> GetAllFromAno(int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.PetRepo.Find(b => b.Date.Year == ano);
            }
        }

        public override string ParseToString()
        {
            return Util.GetDiaMes(this.Date) + "\t" + this.Subject + "\t" + this.Description;
        }

        public static void Consolidate(string fileDir, int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allPet = unitOfWork.PetRepo.Find(b => b.Date.Year == ano).ToList();
                EscreveConsolidadasNoArquivo(fileDir + "Pet" + ano + ".txt", allPet.Cast<SingleDayActivity>().ToList());
            }
        }
    }
}

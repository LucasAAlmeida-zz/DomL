using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.DataAccess;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DomL.Business.Activities.SingleDayActivities
{
    [Table("Purchase")]
    public class Purchase : SingleDayActivity
    {
        [Required]
        public string Loja { get; set; }
        [Required]
        public string Valor { get; set; }

        public Purchase(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }

        protected override void PopulateActivity(IReadOnlyList<string> segmentos)
        {
            //COMPRA; (Loja); (Assunto) O que comprei; (Valor) Quanto custou
            //COMPRA; (Loja); (Assunto) O que comprei; (Valor) Quanto custou; (Descrição) Misc

            this.Loja = segmentos[1];
            this.Subject = segmentos[2];
            this.Valor = segmentos[3];
            if (segmentos.Count == 5)
            {
                this.Description = segmentos[4];
            }
        }

        public static IEnumerable<Purchase> GetAllFromMes(int mes, int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.PurchaseRepo.Find(b => b.Date.Month == mes && b.Date.Year == ano);
            }
        }

        public static IEnumerable<Purchase> GetAllFromAno(int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.PurchaseRepo.Find(b => b.Date.Year == ano);
            }
        }

        public override void Save()
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                if (unitOfWork.PurchaseRepo.Exists(b => b.Date == this.Date)) {
                    return;
                }

                unitOfWork.PurchaseRepo.Add(this);
                unitOfWork.Complete();
            }
        }

        public override string ParseToString()
        {
            return Util.GetDiaMes(this.Date) + "\t" + this.Loja + "\t" + this.Subject + "\t" + this.Valor + "\t" + this.Description;
        }

        public static void Consolidate(string fileDir, int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allPurchase = unitOfWork.PurchaseRepo.Find(b => b.Date.Year == ano).ToList();
                EscreveConsolidadasNoArquivo(fileDir + "Purchase" + ano + ".txt", allPurchase.Cast<SingleDayActivity>().ToList());
            }
        }

        public static int CountYear(int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.PurchaseRepo.Find(g => g.Date.Year == ano).Count();
            }
        }
    }
}

using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DomL.Business.Activities.SingleDayActivities
{
    [Table("Purchase")]
    public class Purchase : SingleDayActivity
    {
        [Required]
        public string Loja { get; set; }
        [Required]
        public int Valor { get; set; }

        public Purchase(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }
        public Purchase() { }

        protected override void PopulateActivity(IReadOnlyList<string> segmentos)
        {
            //COMPRA; (Loja); (Assunto) O que comprei; (Valor) Quanto custou
            //COMPRA; (Loja); (Assunto) O que comprei; (Valor) Quanto custou; (Descrição) Misc

            this.Loja = segmentos[1];
            this.Subject = segmentos[2];
            this.Valor = int.Parse(segmentos[3]);
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

        public static void ConsolidateYear(string fileDir, int year)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allPurchase = unitOfWork.PurchaseRepo.Find(b => b.Date.Year == year).ToList();
                EscreveConsolidadasNoArquivo(fileDir + "Purchase" + year + ".txt", allPurchase.Cast<SingleDayActivity>().ToList());
            }
        }

        public static int CountYear(int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.PurchaseRepo.Find(g => g.Date.Year == ano).Count();
            }
        }

        public static void ConsolidateAll(string fileDir)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allPurchase = unitOfWork.PurchaseRepo.GetAll().ToList();
                EscreveConsolidadasNoArquivo(fileDir + "Purchase.txt", allPurchase.Cast<SingleDayActivity>().ToList());
            }
        }

        public static void FullRestoreFromFile(string fileDir)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allPurchases = GetPurchasesFromFile(fileDir + "Purchase.txt");
                unitOfWork.PurchaseRepo.AddRange(allPurchases);
                unitOfWork.Complete();
            }
        }

        private static List<Purchase> GetPurchasesFromFile(string filePath)
        {
            if (!File.Exists(filePath)) {
                return null;
            }

            var purchases = new List<Purchase>();
            using (var reader = new StreamReader(filePath)) {

                string line;
                while ((line = reader.ReadLine()) != null) {
                    var segmentos = Regex.Split(line, "\t");

                    // Data; (Loja); (Assunto) O que comprei; (Valor) Quanto custou; (Descrição) Misc


                    var purchase = new Purchase() {
                        Date = DateTime.Parse(segmentos[0]),
                        Loja = segmentos[1],
                        Subject = segmentos[2],
                        Valor = int.Parse(segmentos[3]),
                        Description = segmentos[4],

                        DayOrder = 0,
                    };
                    purchases.Add(purchase);
                }
            }
            return purchases;
        }
    }
}

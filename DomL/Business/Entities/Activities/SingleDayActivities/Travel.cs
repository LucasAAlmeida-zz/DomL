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
    [Table("Travel")]
    public class Travel : SingleDayActivity
    {
        [Required]
        public string MeioTransporte { get; set; }

        public Travel(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }
        public Travel() { }

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

        public static void ConsolidateYear(string fileDir, int year)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allTravel = unitOfWork.TravelRepo.Find(b => b.Date.Year == year).ToList();
                EscreveConsolidadasNoArquivo(fileDir + "Travel" + year + ".txt", allTravel.Cast<SingleDayActivity>().ToList());
            }
        }

        public static int CountYear(int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.TravelRepo.Find(g => g.Date.Year == ano).Count();
            }
        }

        public static void ConsolidateAll(string fileDir)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allTravel = unitOfWork.TravelRepo.GetAll().ToList();
                EscreveConsolidadasNoArquivo(fileDir + "Travel.txt", allTravel.Cast<SingleDayActivity>().ToList());
            }
        }

        public static void FullRestoreFromFile(string fileDir)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allTravels = GetTravelsFromFile(fileDir + "Travel.txt");
                unitOfWork.TravelRepo.AddRange(allTravels);
                unitOfWork.Complete();
            }
        }

        private static List<Travel> GetTravelsFromFile(string filePath)
        {
            if (!File.Exists(filePath)) {
                return null;
            }

            var travels = new List<Travel>();
            using (var reader = new StreamReader(filePath)) {

                string line;
                while ((line = reader.ReadLine()) != null) {
                    var segmentos = Regex.Split(line, "\t");

                    // Data; (Assunto) De onde pra onde; (MeioTransporte); (Descrição) o que aconteceu

                    var travel = new Travel() {
                        Date = DateTime.Parse(segmentos[0]),
                        Subject = segmentos[1],
                        MeioTransporte = segmentos[2],
                        Description = segmentos[3],

                        DayOrder = 0,
                    };
                    travels.Add(travel);
                }
            }
            return travels;
        }
    }
}

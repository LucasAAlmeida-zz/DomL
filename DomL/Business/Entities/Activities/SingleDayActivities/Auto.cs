using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DomL.Business.Activities.SingleDayActivities
{
    [Table("Auto")]
    public class Auto : SingleDayActivity
    {
        public Auto(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }
        public Auto() { }

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

        public static void ConsolidateYear(string fileDir, int year)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allAutos = unitOfWork.AutoRepo.Find(b => b.Date.Year == year).ToList();
                EscreveConsolidadasNoArquivo(fileDir + "Auto" + year + ".txt", allAutos.Cast<SingleDayActivity>().ToList());
            }
        }

        public static void ConsolidateAll(string fileDir)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allAutos = unitOfWork.AutoRepo.GetAll().ToList();
                EscreveConsolidadasNoArquivo(fileDir + "Auto.txt", allAutos.Cast<SingleDayActivity>().ToList());
            }
        }

        public static void FullRestoreFromFile(string fileDir)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allAutos = GetAutosFromFile(fileDir + "Auto.txt");
                unitOfWork.AutoRepo.AddRange(allAutos);
                unitOfWork.Complete();
            }
        }

        private static List<Auto> GetAutosFromFile(string filePath)
        {
            if (!File.Exists(filePath)) {
                return null;
            }

            var autos = new List<Auto>();
            using (var reader = new StreamReader(filePath)) {

                string line;
                while ((line = reader.ReadLine()) != null) {
                    var segmentos = Regex.Split(line, "\t");

                    // Data; (Assunto) Qual automovel; (Descricao) O que Aconteceu

                    var auto = new Auto() {
                        Date = DateTime.Parse(segmentos[0]),
                        Subject = segmentos[1],
                        Description = segmentos[2],

                        DayOrder = 0,
                    };
                    autos.Add(auto);
                }
            }
            return autos;
        }
    }
}

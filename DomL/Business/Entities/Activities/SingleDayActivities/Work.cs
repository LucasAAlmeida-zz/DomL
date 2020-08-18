//using DomL.Business.Utils;
//using DomL.Business.Utils.DTOs;
//using DomL.DataAccess;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.IO;
//using System.Linq;
//using System.Text.RegularExpressions;

//namespace DomL.Business.Activities.SingleDayActivities
//{
//    [Table("Work")]
//    public class Work : SingleDayActivity
//    {
//        public Work(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }
//        public Work() { }

//        protected override void PopulateActivity(IReadOnlyList<string> segmentos)
//        {
//            //WORK; (Subject) Qual Trabalho; (Descrição) O que aconteceu

//            this.Subject = segmentos[1];
//            this.Description = segmentos[2];
//        }

//        public override void Save()
//        {
//            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
//                if (unitOfWork.WorkRepo.Exists(b => b.Date == this.Date)) {
//                    return;
//                }

//                unitOfWork.WorkRepo.Add(this);
//                unitOfWork.Complete();
//            }
//        }

//        public static IEnumerable<Work> GetAllFromMes(int mes, int ano)
//        {
//            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
//                return unitOfWork.WorkRepo.Find(b =>b.Date.Month == mes && b.Date.Year == ano);
//            }
//        }

//        public static IEnumerable<Work> GetAllFromAno(int ano)
//        {
//            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
//                return unitOfWork.WorkRepo.Find(b => b.Date.Year == ano);
//            }
//        }

//        public override string ParseToString()
//        {
//            return Util.GetDiaMes(this.Date) + "\t" + this.Description;
//        }

//        public static void ConsolidateYear(string fileDir, int year)
//        {
//            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
//                var allWork = unitOfWork.WorkRepo.Find(b => b.Date.Year == year).ToList();
//                EscreveConsolidadasNoArquivo(fileDir + "Work" + year + ".txt", allWork.Cast<SingleDayActivity>().ToList());
//            }
//        }

//        public static void ConsolidateAll(string fileDir)
//        {
//            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
//                var allWork = unitOfWork.WorkRepo.GetAll().ToList();
//                EscreveConsolidadasNoArquivo(fileDir + "Work.txt", allWork.Cast<SingleDayActivity>().ToList());
//            }
//        }

//        public static void FullRestoreFromFile(string fileDir)
//        {
//            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
//                var allWorks = GetWorksFromFile(fileDir + "Work.txt");
//                unitOfWork.WorkRepo.AddRange(allWorks);
//                unitOfWork.Complete();
//            }
//        }

//        private static List<Work> GetWorksFromFile(string filePath)
//        {
//            if (!File.Exists(filePath)) {
//                return null;
//            }

//            var works = new List<Work>();
//            using (var reader = new StreamReader(filePath)) {

//                string line;
//                while ((line = reader.ReadLine()) != null) {
//                    var segmentos = Regex.Split(line, "\t");

//                    //WORK; (Subject) Qual Trabalho; (Descrição) O que aconteceu

//                    var work = new Work() {
//                        Date = DateTime.Parse(segmentos[0]),
//                        Subject = segmentos[1],
//                        Description = segmentos[2],

//                        DayOrder = 0,
//                    };
//                    works.Add(work);
//                }
//            }
//            return works;
//        }
//    }
//}

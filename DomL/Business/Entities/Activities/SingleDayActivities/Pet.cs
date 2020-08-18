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
//    [Table("Pet")]
//    public class Pet : SingleDayActivity
//    {
//        public Pet(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }
//        public Pet() { }

//        protected override void PopulateActivity(IReadOnlyList<string> segmentos)
//        {
//            //PET; (Assunto) Qual Pet; (Descricao) O que Aconteceu

//            this.Subject = segmentos[1];
//            this.Description = segmentos[2];
//        }

//        public override void Save()
//        {
//            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
//                if (unitOfWork.PetRepo.Exists(b => b.Date == this.Date)) {
//                    return;
//                }

//                unitOfWork.PetRepo.Add(this);
//                unitOfWork.Complete();
//            }
//        }

//        public static IEnumerable<Pet> GetAllFromMes(int mes, int ano)
//        {
//            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
//                return unitOfWork.PetRepo.Find(b => b.Date.Month == mes && b.Date.Year == ano);
//            }
//        }

//        public override string ParseToString()
//        {
//            return Util.GetDiaMes(this.Date) + "\t" + this.Subject + "\t" + this.Description;
//        }

//        public static void ConsolidateYear(string fileDir, int year)
//        {
//            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
//                var allPet = unitOfWork.PetRepo.Find(b => b.Date.Year == year).ToList();
//                EscreveConsolidadasNoArquivo(fileDir + "Pet" + year + ".txt", allPet.Cast<SingleDayActivity>().ToList());
//            }
//        }

//        public static void ConsolidateAll(string fileDir)
//        {
//            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
//                var allPet = unitOfWork.PetRepo.GetAll().ToList();
//                EscreveConsolidadasNoArquivo(fileDir + "Pet.txt", allPet.Cast<SingleDayActivity>().ToList());
//            }
//        }

//        public static void FullRestoreFromFile(string fileDir)
//        {
//            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
//                var allPets = GetPetsFromFile(fileDir + "Pet.txt");
//                unitOfWork.PetRepo.AddRange(allPets);
//                unitOfWork.Complete();
//            }
//        }

//        private static List<Pet> GetPetsFromFile(string filePath)
//        {
//            if (!File.Exists(filePath)) {
//                return null;
//            }

//            var pets = new List<Pet>();
//            using (var reader = new StreamReader(filePath)) {

//                string line;
//                while ((line = reader.ReadLine()) != null) {
//                    var segmentos = Regex.Split(line, "\t");

//                    // Data; (Assunto) Qual Pet; (Descricao) O que Aconteceu

//                    var pet = new Pet() {
//                        Date = DateTime.Parse(segmentos[0]),
//                        Subject = segmentos[1],
//                        Description = segmentos[2],

//                        DayOrder = 0,
//                    };
//                    pets.Add(pet);
//                }
//            }
//            return pets;
//        }
//    }
//}

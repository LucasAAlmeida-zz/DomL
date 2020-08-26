using DomL.Business.Utils;

using DomL.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DomL.Business.Entities
{
    [Table("HealthActivity")]
    public class HealthActivity
    {
        [Key]
        [ForeignKey("Activity")]
        public int Id { get; set; }
        [ForeignKey("Specialty")]
        public int? SpecialtyId { get; set; }
        [Required]
        public string Description { get; set; }

        public virtual Activity Activity { get; set; }
        public virtual MedicalSpecialty Specialty { get; set; }
    }

    [Table("MedicalSpecialty")]
    public class MedicalSpecialty
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
    //    public Health(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }
    //    public Health() { }

    //    protected override void PopulateActivity(IReadOnlyList<string> segmentos)
    //    {
    //        //SAUDE; (Descrição) o que aconteceu
    //        //SAUDE; (Assunto) Especialidade médica; (Descrição) o que aconteceu

    //        if (segmentos.Count == 2) {
    //            this.Description = segmentos[1];
    //        } else {
    //            this.Subject = segmentos[1];
    //            this.Description = segmentos[2];
    //        }
    //    }

    //    public override void Save()
    //    {
    //        using (var unitOfWork = new UnitOfWork(new DomLContext())) {
    //            if (unitOfWork.HealthRepo.Exists(b => b.Date == this.Date)) {
    //                return;
    //            }

    //            unitOfWork.HealthRepo.Add(this);
    //            unitOfWork.Complete();
    //        }
    //    }

    //    public override string ParseToString()
    //    {
    //        string assunto = !string.IsNullOrWhiteSpace(this.Subject) ? this.Subject : "-";
    //        return Util.GetDiaMes(this.Date) + "\t" + assunto + "\t" + this.Description;
    //    }
    //    public static IEnumerable<Health> GetAllFromMes(int mes, int ano)
    //    {
    //        using (var unitOfWork = new UnitOfWork(new DomLContext())) {
    //            return unitOfWork.HealthRepo.Find(b => b.Date.Month == mes && b.Date.Year == ano);
    //        }
    //    }

    //    public static void ConsolidateYear(string fileDir, int year)
    //    {
    //        using (var unitOfWork = new UnitOfWork(new DomLContext())) {
    //            var allHealth = unitOfWork.HealthRepo.Find(b => b.Date.Year == year).ToList();
    //            EscreveConsolidadasNoArquivo(fileDir + "Health" + year + ".txt", allHealth.Cast<SingleDayActivity>().ToList());
    //        }
    //    }

    //    public static void ConsolidateAll(string fileDir)
    //    {
    //        using (var unitOfWork = new UnitOfWork(new DomLContext())) {
    //            var allHealth = unitOfWork.HealthRepo.GetAll().ToList();
    //            EscreveConsolidadasNoArquivo(fileDir + "Health.txt", allHealth.Cast<SingleDayActivity>().ToList());
    //        }
    //    }

    //    public static void FullRestoreFromFile(string fileDir)
    //    {
    //        using (var unitOfWork = new UnitOfWork(new DomLContext())) {
    //            var allHealths = GetHealthsFromFile(fileDir + "Health.txt");
    //            unitOfWork.HealthRepo.AddRange(allHealths);
    //            unitOfWork.Complete();
    //        }
    //    }

    //    private static List<Health> GetHealthsFromFile(string filePath)
    //    {
    //        if (!File.Exists(filePath)) {
    //            return null;
    //        }

    //        var healths = new List<Health>();
    //        using (var reader = new StreamReader(filePath)) {

    //            string line;
    //            while ((line = reader.ReadLine()) != null) {
    //                var segmentos = Regex.Split(line, "\t");

    //                // Data; (Assunto) Especialidade médica; (Descrição) o que aconteceu

    //                var health = new Health() {
    //                    Date = DateTime.Parse(segmentos[0]),
    //                    Subject = segmentos[1],
    //                    Description = segmentos[2],

    //                    DayOrder = 0,
    //                };
    //                healths.Add(health);
    //            }
    //        }
    //        return healths;
    //    }
    //}
}

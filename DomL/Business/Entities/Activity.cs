using DomL.Business.Utils.Enums;
using DomL.DataAccess;
using DomL.DataAccess.Repositories;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Entities
{
    [Table("Activity")]
    public class Activity
    {
        [Key]
        public int Id { get; set; }
        public Category Category { get; set; }
        public DateTime Date { get; set; }
        public int DayOrder { get; set; }
        public Classification Classification { get; set; }
        public int? ActivityBlockId { get; set; }

        [ForeignKey("ActivityBlockId")]
        public ActivityBlock ActivityBlock { get; set; }

        //[MaxLength(255)]
        //public string Subject { get; set; }
        //[MaxLength(1000)]
        //public string Description { get; set; }
    }

    [Table("ActivityBlock")]
    public class ActivityBlock
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        //public int? Save()
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        if (unitOfWork.ActivityBlockRepo.Exists(b => b.Name == this.Name)) {
        //            return null;
        //        }

        //        unitOfWork.ActivityBlockRepo.Add(this);
        //        unitOfWork.Complete();

        //        this.Id = unitOfWork.ActivityBlockRepo.SingleOrDefault(abr => abr.Name == this.Name).Id;
        //        return this.Id;
        //    }
        //}

        //public static void ConsolidateYear(string fileDir, int year)
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        var allActivitiesInBlocksInYear = unitOfWork.ActivityBlockRepo.GetActivitiesInBlockYear(year).ToList();
        //        using (var file = new StreamWriter(fileDir + "ActivityBlocks" + year + ".txt")) {
        //            foreach (var atividade in allActivitiesInBlocksInYear) {
        //                file.WriteLine(atividade.ParseToString());
        //            }
        //        }
        //    }
        //}
    }
}

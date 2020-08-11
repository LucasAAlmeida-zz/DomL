using DomL.Business.Activities.MultipleDayActivities;
using DomL.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;

namespace DomL.Business
{
    [Table("ActivityBlock")]
    public class ActivityBlock
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public int Year { get; set; }

        public virtual List<Book> BookList { get; set; }

        public ActivityBlock(string name, int year)
        {
            this.Name = name;
            this.Year = year;
        }

        public int? Save()
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                if (unitOfWork.ActivityBlockRepo.Exists(b => b.Name == this.Name)) {
                    return null;
                }

                unitOfWork.ActivityBlockRepo.Add(this);
                unitOfWork.Complete();

                this.Id = unitOfWork.ActivityBlockRepo.SingleOrDefault(abr => abr.Name == this.Name).Id;
                return this.Id;
            }
        }

        public static void Consolidate(string fileDir, int year)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allActivitiesInBlocksInYear = unitOfWork.ActivityBlockRepo.GetActivitiesInBlockYear(year).ToList();
                using (var file = new StreamWriter(fileDir + "ActivityBlocks" + year + ".txt")) {
                    foreach (var atividade in allActivitiesInBlocksInYear) {
                        file.WriteLine(atividade.ParseToString());
                    }
                }
            }
        }
    }
}

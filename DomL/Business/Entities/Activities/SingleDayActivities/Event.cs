using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.DataAccess;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DomL.Business.Activities.SingleDayActivities
{
    [Table("Event")]
    public class Event : SingleDayActivity
    {
        [Required]
        public bool IsImportant { get; set; }

        [NotMapped]
        public new string Subject { get; set; }


        public Event(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }

        protected override void PopulateActivity(IReadOnlyList<string> segments)
        {
            // (Descricao)
            this.Description = segments[0];
            this.IsImportant = false;

            if (this.Description.StartsWith("*")) {
                this.IsImportant = true;
                this.Description = this.Description.Substring(1);
            } else if (this.Description.StartsWith("<") && !this.Description.StartsWith("<END>")) {
                this.IsImportant = true;
            }
        }

        public override void Save()
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                if (unitOfWork.EventRepo.Exists(b => b.Date == this.Date && b.Description == this.Description)) {
                    return;
                }

                unitOfWork.EventRepo.Add(this);
                unitOfWork.Complete();
            }
        }

        public override string ParseToString()
        {
            return Util.GetDiaMes(this.Date) + "\t" + this.Description;
        }

        public static IEnumerable<Event> GetImportantFromMes(int mes, int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.EventRepo
                    .Find(b =>
                        b.Date.Month == mes && b.Date.Year == ano
                        && (b.IsImportant || b.ActivityBlockId != null)
                    );
            }
        }

        public static void ConsolidateYear(string fileDir, int year)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allImportantEvents = unitOfWork.EventRepo.Find(b => b.Date.Year == year && b.IsImportant).ToList();
                EscreveConsolidadasNoArquivo(fileDir + "Event" + year + ".txt", allImportantEvents.Cast<SingleDayActivity>().ToList());
            }
        }
    }
}

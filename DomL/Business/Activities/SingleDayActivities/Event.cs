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

        public static IEnumerable<Event> GetImportantFromMesAno(int mes, int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.EventRepo
                    .Find(b =>
                        b.Date.Month == mes && b.Date.Year == ano
                        && (b.IsImportant || b.ActivityBlockId != null)
                    );
            }
        }

        public static IEnumerable<Event> GetImportantFromAno(int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.EventRepo.Find(b => b.Date.Year == ano && (b.IsImportant || b.ActivityBlockId != null));
            }
        }

        public override string ParseToString()
        {
            return Util.GetDiaMes(this.Date) + "\t" + this.Description;
        }

        public static void Consolidate(string fileDir, int year)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allImportantEvents = unitOfWork.EventRepo.Find(b => b.Date.Year == year && b.IsImportant).ToList();
                EscreveConsolidadasNoArquivo(fileDir + "Event" + year + ".txt", allImportantEvents.Cast<SingleDayActivity>().ToList());
            }
        }
    }
}

//namespace DomL.Business.Activities.SpecialActivities
//{
//    public class Event : BlockActivity
//    {
//        public Event(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }

//        public bool shouldSave = false;

//        protected override void PopulateActivity(IReadOnlyList<string> segmentos)
//        {
//            this.Description = segmentos[0];
//            this.FullLine = this.Description;

//            if (this.Description.StartsWith("*")) {
//                this.Description = this.Description.Substring(1);
//                this.shouldSave = true;
//            } else if (this.Description.StartsWith("<")) {
//                this.IsInSpecialBlock = true;
//            } else if (this.Description.StartsWith("---")) {
//                this.IsInSpecialBlock = false;
//            }
//        }

//        protected override void ParseAtividadeVelha(string[] segmentos)
//        {
//            this.Description = segmentos[1];
//            this.FullLine = this.Description;
//        }

//        public new static void Consolidate(List<Activity> newEventActivities, string fileDir, int year)
//        {
//            string filePath = fileDir + "Events.txt";
//            var atividadesVelhas = GetAtividadesVelhas(filePath, year);
//            atividadesVelhas.AddRange(Util.GetAtividadesToAdd(newEventActivities, atividadesVelhas));
//            var allAtividadesCategoria = atividadesVelhas;
//            EscreverNoArquivo(filePath, allAtividadesCategoria);
//        }

//        public static List<Activity> GetAtividadesVelhas(string filePath, int year)
//        {
//            var atividadesVelhas = new List<Activity>();

//            if (File.Exists(filePath)) {
//                using (var reader = new StreamReader(filePath)) {
//                    string line;
//                    while ((line = reader.ReadLine()) != null) {
//                        var segmentos = Regex.Split(line, "\t");

//                        ActivityDTO atividadeVelhaDTO = Util.GetAtividadeVelha(segmentos[0], year);

//                        atividadesVelhas.Add(new Event(atividadeVelhaDTO, segmentos));
//                    }
//                }
//            }

//            return atividadesVelhas;
//        }

//        private static void EscreverNoArquivo(string filePath, List<Activity> allAtividadesCategoria)
//        {
//            using (var file = new StreamWriter(filePath)) {
//                foreach (Event atividade in allAtividadesCategoria) {
//                    if (atividade.IsInSpecialBlock && !atividade.FullLine.StartsWith("<")) {
//                        continue;
//                    }

//                    var consolidatedActivity = atividade.ParseToString();
//                    file.WriteLine(consolidatedActivity);
//                }
//            }
//        }

//        public override string ParseToString()
//        {
//            return Util.GetDiaMes(this.Date) + "\t" + this.FullLine;
//        }
//    }
//}

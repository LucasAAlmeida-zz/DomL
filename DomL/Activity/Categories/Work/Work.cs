using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Entities
{
    [Table("WorkActivity")]
    public class WorkActivity
    {
        [Key]
        [ForeignKey("Activity")]
        public int Id { get; set; }
        [ForeignKey("Work")]
        public int WorkId { get; set; }
        [Required]
        public string Description { get; set; }

        public virtual Activity Activity { get; set; }
        public virtual Company Work { get; set; }
    }

        //protected override void PopulateActivity(IReadOnlyList<string> segmentos)
        //{
        //    

        //    this.Subject = segmentos[1];
        //    this.Description = segmentos[2];
        //}

        //public override void Save()
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        if (unitOfWork.WorkRepo.Exists(b => b.Date == this.Date)) {
        //            return;
        //        }

        //        unitOfWork.WorkRepo.Add(this);
        //        unitOfWork.Complete();
        //    }
        //}

        //public override string ParseToString()
        //{
        //    return Util.GetDiaMes(this.Date) + "\t" + this.Subject + "\t" + this.Description;
        //}

        //public static IEnumerable<Work> GetAllFromMes(int mes, int ano)
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        return unitOfWork.WorkRepo.Find(b => b.Date.Month == mes && b.Date.Year == ano);
        //    }
        //}

        //public static void ConsolidateYear(string fileDir, int year)
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        var allWorks = unitOfWork.WorkRepo.Find(b => b.Date.Year == year).ToList();
        //        EscreveConsolidadasNoArquivo(fileDir + "Work" + year + ".txt", allWorks.Cast<SingleDayActivity>().ToList());
        //    }
        //}

        //public static void ConsolidateAll(string fileDir)
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        var allWorks = unitOfWork.WorkRepo.GetAll().ToList();
        //        EscreveConsolidadasNoArquivo(fileDir + "Work.txt", allWorks.Cast<SingleDayActivity>().ToList());
        //    }
        //}

        //public static void FullRestoreFromFile(string fileDir)
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        var allWorks = GetWorksFromFile(fileDir + "Work.txt");
        //        unitOfWork.WorkRepo.AddRange(allWorks);
        //        unitOfWork.Complete();
        //    }
        //}

        //private static List<Work> GetWorksFromFile(string filePath)
        //{
        //    if (!File.Exists(filePath)) {
        //        return null;
        //    }

        //    var works = new List<Work>();
        //    using (var reader = new StreamReader(filePath)) {

        //        string line;
        //        while ((line = reader.ReadLine()) != null) {
        //            var segmentos = Regex.Split(line, "\t");

        //            // Data; (Assunto) Qual workmovel; (Descricao) O que Aconteceu

        //            var work = new Work() {
        //                Date = DateTime.Parse(segmentos[0]),
        //                Subject = segmentos[1],
        //                Description = segmentos[2],

        //                DayOrder = 0,
        //            };
        //            works.Add(work);
        //        }
        //    }
        //    return works;
        //}
}

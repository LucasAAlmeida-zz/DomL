using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Entities
{
    [Table("MeetActivity")]
    public class MeetActivity
    {
        [Key]
        [ForeignKey("Activity")]
        public int Id { get; set; }
        [ForeignKey("Person")]
        public int PersonId { get; set; }
        [Required]
        public string Description { get; set; }

        public virtual Activity Activity { get; set; }
        public virtual Person Person { get; set; }
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
        //        if (unitOfWork.MeetRepo.Exists(b => b.Date == this.Date)) {
        //            return;
        //        }

        //        unitOfWork.MeetRepo.Add(this);
        //        unitOfWork.Complete();
        //    }
        //}

        //public override string ParseToString()
        //{
        //    return Util.GetDiaMes(this.Date) + "\t" + this.Subject + "\t" + this.Description;
        //}

        //public static IEnumerable<Meet> GetAllFromMes(int mes, int ano)
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        return unitOfWork.MeetRepo.Find(b => b.Date.Month == mes && b.Date.Year == ano);
        //    }
        //}

        //public static void ConsolidateYear(string fileDir, int year)
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        var allMeets = unitOfWork.MeetRepo.Find(b => b.Date.Year == year).ToList();
        //        EscreveConsolidadasNoArquivo(fileDir + "Meet" + year + ".txt", allMeets.Cast<SingleDayActivity>().ToList());
        //    }
        //}

        //public static void ConsolidateAll(string fileDir)
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        var allMeets = unitOfWork.MeetRepo.GetAll().ToList();
        //        EscreveConsolidadasNoArquivo(fileDir + "Meet.txt", allMeets.Cast<SingleDayActivity>().ToList());
        //    }
        //}

        //public static void FullRestoreFromFile(string fileDir)
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        var allMeets = GetMeetsFromFile(fileDir + "Meet.txt");
        //        unitOfWork.MeetRepo.AddRange(allMeets);
        //        unitOfWork.Complete();
        //    }
        //}

        //private static List<Meet> GetMeetsFromFile(string filePath)
        //{
        //    if (!File.Exists(filePath)) {
        //        return null;
        //    }

        //    var meets = new List<Meet>();
        //    using (var reader = new StreamReader(filePath)) {

        //        string line;
        //        while ((line = reader.ReadLine()) != null) {
        //            var segmentos = Regex.Split(line, "\t");

        //            // Data; (Assunto) Qual meetmovel; (Descricao) O que Aconteceu

        //            var meet = new Meet() {
        //                Date = DateTime.Parse(segmentos[0]),
        //                Subject = segmentos[1],
        //                Description = segmentos[2],

        //                DayOrder = 0,
        //            };
        //            meets.Add(meet);
        //        }
        //    }
        //    return meets;
        //}
}

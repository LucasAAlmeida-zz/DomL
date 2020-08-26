using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Entities
{
    [Table("PlayActivity")]
    public class PlayActivity
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
        //        if (unitOfWork.PlayRepo.Exists(b => b.Date == this.Date)) {
        //            return;
        //        }

        //        unitOfWork.PlayRepo.Add(this);
        //        unitOfWork.Complete();
        //    }
        //}

        //public override string ParseToString()
        //{
        //    return Util.GetDiaMes(this.Date) + "\t" + this.Subject + "\t" + this.Description;
        //}

        //public static IEnumerable<Play> GetAllFromMes(int mes, int ano)
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        return unitOfWork.PlayRepo.Find(b => b.Date.Month == mes && b.Date.Year == ano);
        //    }
        //}

        //public static void ConsolidateYear(string fileDir, int year)
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        var allPlays = unitOfWork.PlayRepo.Find(b => b.Date.Year == year).ToList();
        //        EscreveConsolidadasNoArquivo(fileDir + "Play" + year + ".txt", allPlays.Cast<SingleDayActivity>().ToList());
        //    }
        //}

        //public static void ConsolidateAll(string fileDir)
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        var allPlays = unitOfWork.PlayRepo.GetAll().ToList();
        //        EscreveConsolidadasNoArquivo(fileDir + "Play.txt", allPlays.Cast<SingleDayActivity>().ToList());
        //    }
        //}

        //public static void FullRestoreFromFile(string fileDir)
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        var allPlays = GetPlaysFromFile(fileDir + "Play.txt");
        //        unitOfWork.PlayRepo.AddRange(allPlays);
        //        unitOfWork.Complete();
        //    }
        //}

        //private static List<Play> GetPlaysFromFile(string filePath)
        //{
        //    if (!File.Exists(filePath)) {
        //        return null;
        //    }

        //    var plays = new List<Play>();
        //    using (var reader = new StreamReader(filePath)) {

        //        string line;
        //        while ((line = reader.ReadLine()) != null) {
        //            var segmentos = Regex.Split(line, "\t");

        //            // Data; (Assunto) Qual playmovel; (Descricao) O que Aconteceu

        //            var play = new Play() {
        //                Date = DateTime.Parse(segmentos[0]),
        //                Subject = segmentos[1],
        //                Description = segmentos[2],

        //                DayOrder = 0,
        //            };
        //            plays.Add(play);
        //        }
        //    }
        //    return plays;
        //}
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Entities
{
    [Table("ShowActivity")]
    public class ShowActivity
    {
        [Key]
        [ForeignKey("Activity")]
        public int Id { get; set; }
        [ForeignKey("ShowSeason")]
        public int ShowSeasonId { get; set; }

        public string Description { get; set; }

        public virtual Activity Activity { get; set; }
        public virtual ShowSeason ShowSeason { get; set; }
    }

    [Table("ShowSeason")]
    public class ShowSeason
    {
        [Key]
        public int Id { get; set; }
        public int SeriesId { get; set; }
        public string Season { get; set; }
        public int? DirectorId { get; set; }
        public int? TypeId { get; set; }
        public string Score { get; set; }

        [ForeignKey("SeriesId")]
        public Series Series { get; set; }
        [ForeignKey("DirectorId")]
        public Person Director { get; set; }
        [ForeignKey("TypeId")]
        public MediaType Type { get; set; }

        

        //public override void Save()
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        if (unitOfWork.ShowRepo.Exists(b => b.Date == this.Date && b.Subject == this.Subject)) {
        //            return;
        //        }

        //        unitOfWork.ShowRepo.Add(this);
        //        unitOfWork.Complete();
        //    }
        //}

        //public static IEnumerable<Show> GetAllFromMes(int mes, int ano)
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        return unitOfWork.ShowRepo.Find(b => b.Date.Month == mes && b.Date.Year == ano);
        //    }
        //}

        //public static void ConsolidateYear(string fileDir, int year)
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        var allShows = unitOfWork.ShowRepo.Find(b => b.Date.Year == year).ToList();
        //        EscreveConsolidadasNoArquivo(fileDir + "Show" + year + ".txt", allShows.Cast<MultipleDayActivity>().ToList());
        //    }
        //}

        //public static void ConsolidateAll(string fileDir)
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        var allShows = unitOfWork.ShowRepo.GetAll().ToList();
        //        EscreveConsolidadasNoArquivo(fileDir + "Show.txt", allShows.Cast<MultipleDayActivity>().ToList());
        //    }
        //}

        //public static int CountEndedYear(int ano)
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        return unitOfWork.ShowRepo
        //            .Find(g =>
        //                (g.Classificacao == Classification.Termino || g.Classificacao == Classification.Unica)
        //                && g.Date.Year == ano)
        //            .Count();
        //    }
        //}

        //public static void FullRestoreFromFile(string fileDir)
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        var allShows = GetShowsFromFile(fileDir + "Show.txt");
        //        unitOfWork.ShowRepo.AddRange(allShows);
        //        unitOfWork.Complete();
        //    }
        //}

        //private static List<Show> GetShowsFromFile(string filePath)
        //{
        //    var shows = new List<Show>();
        //    using (var reader = new StreamReader(filePath)) {

        //        string line = "";
        //        try {
        //            while ((line = reader.ReadLine()) != null) {
        //                if (string.IsNullOrWhiteSpace(line)) {
        //                    continue;
        //                }
        //                var segmentos = Regex.Split(line, "\t");

        //                // DataInicio; DataFim; (De Quem); (Assunto); (Nota); (Descrição)

        //                int? nota = segmentos[4] != "-" ? int.Parse(segmentos[4]) : (int?)null;
        //                string descricao = segmentos[5] != "-" ? segmentos[5] : null;

        //                if (segmentos[0] == segmentos[1]) {
        //                    var show = new Show() {
        //                        Date = DateTime.ParseExact(segmentos[0], "dd/MM/yy", null),
        //                        Classificacao = Classification.Unica,
        //                        DeQuem = segmentos[2],
        //                        Subject = segmentos[3],
        //                        Nota = nota,
        //                        Description = descricao,

        //                        DayOrder = 0,
        //                    };
        //                    shows.Add(show);
        //                    continue;
        //                }

        //                if (!segmentos[0].StartsWith("??/??")) {
        //                    var show = new Show() {
        //                        Date = DateTime.ParseExact(segmentos[0], "dd/MM/yy", null),
        //                        Classificacao = Classification.Comeco,
        //                        DeQuem = segmentos[2],
        //                        Subject = segmentos[3],
        //                        Nota = nota,
        //                        Description = segmentos[1].StartsWith("??/??") ? descricao : null,

        //                        DayOrder = 0,
        //                    };
        //                    shows.Add(show);
        //                }

        //                if (!segmentos[1].StartsWith("??/??")) {
        //                    var show = new Show() {
        //                        Date = DateTime.ParseExact(segmentos[1], "dd/MM/yy", null),
        //                        Classificacao = Classification.Termino,
        //                        DeQuem = segmentos[2],
        //                        Subject = segmentos[3],
        //                        Nota = nota,
        //                        Description = descricao,

        //                        DayOrder = 0,
        //                    };
        //                    shows.Add(show);
        //                }
        //            }
        //        } catch (Exception e) {
        //            var msg = "Deu ruim na linha " + line;
        //            throw new ParseException(msg, e);
        //        }
        //    }
        //    return shows;
        //}
    }
}

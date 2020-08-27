using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Entities
{
    [Table("MovieActivity")]
    public class MovieActivity
    {
        [Key]
        [ForeignKey("Activity")]
        public int Id { get; set; }
        [ForeignKey("Movie")]
        public int MovieId { get; set; }

        public string Description { get; set; }

        public virtual Activity Activity { get; set; }
        public virtual Movie Movie { get; set; }
    }

    [Table("Movie")]
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public int? DirectorId { get; set; }
        public int? SeriesId { get; set; }
        public string NumberInSeries { get; set; }
        public string Score { get; set; }

        [ForeignKey("DirectorId")]
        public Person Director { get; set; }
        [ForeignKey("SeriesId")]
        public Series Series { get; set; }
    }
    //    [Required]
    //    public string Nota { get; set; }

    //    public Movie(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }
    //    public Movie() { }

    //    protected override void PopulateActivity(IReadOnlyList<string> segmentos)
    //    {
    //        // FILME; (Assunto) Título; (Nota)
    //        // FILME; (Assunto) Título; (Nota); (Descrição) O que achei

    //        this.Subject = segmentos[1];
    //        this.Nota = segmentos[2];
    //        if (segmentos.Count == 4) {
    //            this.Description = segmentos[3];
    //        }
    //    }

    //    public override void Save()
    //    {
    //        using (var unitOfWork = new UnitOfWork(new DomLContext())) {
    //            if (unitOfWork.MovieRepo.Exists(b => b.Date == this.Date)) {
    //                return;
    //            }

    //            unitOfWork.MovieRepo.Add(this);
    //            unitOfWork.Complete();
    //        }
    //    }

    //    public override string ParseToString()
    //    {
    //        return Util.GetDiaMes(this.Date) + "\t" + this.Subject + "\t" + this.Nota + "\t" + this.Description;
    //    }

    //    public static int CountYear(int ano)
    //    {
    //        using (var unitOfWork = new UnitOfWork(new DomLContext())) {
    //            return unitOfWork.MovieRepo.Find(g => g.Date.Year == ano).Count();
    //        }
    //    }

    //    public static IEnumerable<Movie> GetAllFromMes(int mes, int ano)
    //    {
    //        using (var unitOfWork = new UnitOfWork(new DomLContext())) {
    //            return unitOfWork.MovieRepo.Find(b => b.Date.Month == mes && b.Date.Year == ano);
    //        }
    //    }

    //    public static void ConsolidateYear(string fileDir, int year)
    //    {
    //        using (var unitOfWork = new UnitOfWork(new DomLContext())) {
    //            var allMovie = unitOfWork.MovieRepo.Find(b => b.Date.Year == year).ToList();
    //            EscreveConsolidadasNoArquivo(fileDir + "Movie" + year + ".txt", allMovie.Cast<SingleDayActivity>().ToList());
    //        }
    //    }

    //    public static void ConsolidateAll(string fileDir)
    //    {
    //        using (var unitOfWork = new UnitOfWork(new DomLContext())) {
    //            var allMovie = unitOfWork.MovieRepo.GetAll().ToList();
    //            EscreveConsolidadasNoArquivo(fileDir + "Movie.txt", allMovie.Cast<SingleDayActivity>().ToList());
    //        }
    //    }

    //    public static void FullRestoreFromFile(string fileDir)
    //    {
    //        using (var unitOfWork = new UnitOfWork(new DomLContext())) {
    //            var allMovies = GetMoviesFromFile(fileDir + "Movie.txt");
    //            unitOfWork.MovieRepo.AddRange(allMovies);
    //            unitOfWork.Complete();
    //        }
    //    }

    //    private static List<Movie> GetMoviesFromFile(string filePath)
    //    {
    //        if (!File.Exists(filePath)) {
    //            return null;
    //        }

    //        var movies = new List<Movie>();
    //        using (var reader = new StreamReader(filePath)) {

    //            string line;
    //            while ((line = reader.ReadLine()) != null) {
    //                var segmentos = Regex.Split(line, "\t");

    //                // Data; (Assunto) Título; (Nota); (Descrição) O que achei

    //                var movie = new Movie() {
    //                    Date = DateTime.Parse(segmentos[0]),
    //                    Subject = segmentos[1],
    //                    Nota = segmentos[2],
    //                    Description = segmentos[3],

    //                    DayOrder = 0,
    //                };
    //                movies.Add(movie);
    //            }
    //        }
    //        return movies;
    //    }
    //}
}

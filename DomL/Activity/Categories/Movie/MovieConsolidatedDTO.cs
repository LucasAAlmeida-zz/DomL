using DomL.Business.Entities;
using DomL.Presentation;

namespace DomL.Business.DTOs
{
    public class MovieConsolidatedDTO : ActivityConsolidatedDTO
    {
        public string Title;
        public string Person;
        public string SeriesName;
        public string Number;
        public string Company;
        public string Year;
        public string Score;
        public string Description;

        public MovieConsolidatedDTO(Activity activity) : base(activity)
        {
            CategoryName = "MOVIE";

            var movieActivity = activity.MovieActivity;
            var movie = movieActivity.Movie;
            
            Title = movie.Title;
            Person = movie.Person ?? "-";
            SeriesName = (movie.Series != null) ? movie.Series.Name : "-";
            Number = movie.Number ?? "-";
            Company = movie.Company ?? "-";
            Year = movie.Year.ToString();
            Score = movie.Score ?? "-";
            Description = (!string.IsNullOrWhiteSpace(movieActivity.Description)) ? movieActivity.Description : "-";
        }

        public MovieConsolidatedDTO(MovieWindow movieWindow, Activity activity) : base(activity)
        {
            CategoryName = "MOVIE";

            Title = movieWindow.TitleCB.Text;
            Person = movieWindow.DirectorCB.Text;
            SeriesName = movieWindow.SeriesCB.Text;
            Number = movieWindow.NumberCB.Text;
            Company = movieWindow.CompanyCB.Text;
            Year = movieWindow.YearCB.Text;
            Score = movieWindow.ScoreCB.Text;
            Description = (!string.IsNullOrWhiteSpace(movieWindow.DescriptionCB.Text)) ? movieWindow.DescriptionCB.Text : null;
        }

        public MovieConsolidatedDTO(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "AUTO";

            Title = backupSegments[4];
            Person = backupSegments[5];
            SeriesName = backupSegments[6];
            Number = backupSegments[7];
            Company = backupSegments[8];
            Year = backupSegments[9];
            Score = backupSegments[10];
            Description = backupSegments[11];

            OriginalLine = GetInfoForOriginalLine()
                + GetMovieActivityInfo().Replace("\t", "; ");
        }

        public new string GetInfoForYearRecap()
        {
            return base.GetInfoForYearRecap()
                + "\t" + GetMovieActivityInfo();
        }

        public new string GetInfoForBackup()
        {
            return base.GetInfoForBackup()
                + "\t" + GetMovieActivityInfo();
        }

        public string GetMovieActivityInfo()
        {
            return Title + "\t" + Person
                + "\t" + SeriesName + "\t" + Number
                + "\t" + Company + "\t" + Year
                + "\t" + Score + "\t" + Description;
        }
    }
}

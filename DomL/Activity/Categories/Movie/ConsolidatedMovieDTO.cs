using DomL.Business.Entities;
using DomL.Presentation;

namespace DomL.Business.DTOs
{
    public class ConsolidatedMovieDTO : ConsolidatedActivityDTO
    {
        public string Title;
        public string DirectorName;
        public string SeriesName;
        public string NumberInSeries;
        public string ScoreValue;
        public string Description;

        public ConsolidatedMovieDTO(Activity activity) : base(activity)
        {
            CategoryName = "MOVIE";

            var movieActivity = activity.MovieActivity;
            var movie = movieActivity.Movie;
            
            Title = movie.Title;
            DirectorName = (movie.Director != null) ? movie.Director.Name : "-";
            SeriesName = (movie.Series != null) ? movie.Series.Name : "-";
            NumberInSeries = (!string.IsNullOrWhiteSpace(movie.NumberInSeries)) ? movie.NumberInSeries : "-";
            ScoreValue = (movie.Score != null) ? movie.Score.Value.ToString() : "-";
            Description = (!string.IsNullOrWhiteSpace(movieActivity.Description)) ? movieActivity.Description : "-";
        }

        public ConsolidatedMovieDTO(MovieWindow movieWindow, Activity activity) : base(activity)
        {
            CategoryName = "MOVIE";

            Title = movieWindow.TitleCB.Text;
            DirectorName = movieWindow.DirectorCB.Text;
            SeriesName = movieWindow.SeriesCB.Text;
            NumberInSeries = (!string.IsNullOrWhiteSpace(movieWindow.NumberCB.Text)) ? movieWindow.NumberCB.Text : null;
            ScoreValue = movieWindow.ScoreCB.Text;
            Description = (!string.IsNullOrWhiteSpace(movieWindow.DescriptionCB.Text)) ? movieWindow.DescriptionCB.Text : null;
        }

        public ConsolidatedMovieDTO(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "AUTO";

            Title = backupSegments[4];
            DirectorName = backupSegments[5];
            SeriesName = backupSegments[6];
            NumberInSeries = backupSegments[7];
            ScoreValue = backupSegments[8];
            Description = backupSegments[9];

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
            return Title + "\t" + DirectorName
                + "\t" + SeriesName + "\t" + NumberInSeries
                + "\t" + ScoreValue + "\t" + Description;
        }
    }
}

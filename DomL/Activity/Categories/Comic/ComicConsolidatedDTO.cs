using DomL.Business.Entities;
using DomL.Presentation;

namespace DomL.Business.DTOs
{
    public class ComicConsolidatedDTO : ActivityConsolidatedDTO
    {
        public string Title;
        public string Author;
        public string Type;
        public string SeriesName;
        public string Number;
        public string Publisher;
        public string Year;
        public string Score;
        public string Description;

        public ComicConsolidatedDTO(Activity activity) : base (activity)
        {
            CategoryName = "COMIC";

            var comicActivity = activity.ComicActivity;
            var comic = comicActivity.Comic;

            Title = comic.Title;
            Author = comic.Author ?? "-";
            Type = comic.Type ?? "-";
            SeriesName = comic.Series.Name;
            Number = comic.Number ?? "-";
            Publisher = comic.Publisher ?? "-";
            Year = comic.Year.ToString();
            Score = comic.Score ?? "-";
            Description = comicActivity.Description ?? "-";
        }

        public ComicConsolidatedDTO(ComicWindow comicWindow, Activity activity) : base(activity)
        {
            CategoryName = "COMIC";

            Title = comicWindow.TitleCB.Text;
            Author = comicWindow.AuthorCB.Text;
            Type = comicWindow.TypeCB.Text;
            SeriesName = comicWindow.SeriesCB.Text;
            Number = comicWindow.NumberCB.Text;
            Publisher = comicWindow.PublisherCB.Text;
            Year = comicWindow.YearCB.Text;
            Score = comicWindow.ScoreCB.Text;
            Description = comicWindow.DescriptionCB.Text;
        }

        public ComicConsolidatedDTO(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "COMIC";

            Title = backupSegments[4];
            Author = backupSegments[5];
            Type = backupSegments[6];
            SeriesName = backupSegments[7];
            Number = backupSegments[8];
            Publisher = backupSegments[9];
            Year = backupSegments[10];
            Score = backupSegments[11];
            Description = backupSegments[12];

            OriginalLine = GetInfoForOriginalLine() + "; "
                + GetComicActivityInfo().Replace("\t", "; ");
        }

        public new string GetInfoForYearRecap()
        {
            return base.GetInfoForYearRecap()
                + "\t" + GetComicActivityInfo();
        }

        public new string GetInfoForBackup()
        {
            return base.GetInfoForBackup()
                + "\t" + GetComicActivityInfo();
        }

        private string GetComicActivityInfo()
        {
            return Title
                + "\t" + Author + "\t" + Type
                + "\t" + SeriesName + "\t" + Number
                + "\t" + Publisher + "\t" + Year
                + "\t" + Score + "\t" + Description;
        }
    }
}

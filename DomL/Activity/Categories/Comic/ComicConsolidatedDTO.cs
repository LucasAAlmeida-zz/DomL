using DomL.Business.Entities;
using DomL.Presentation;

namespace DomL.Business.DTOs
{
    public class ComicConsolidatedDTO : ActivityConsolidatedDTO
    {
        public string Title;
        public string Person;
        public string Type;
        public string Series;
        public string Number;
        public string Company;
        public string Year;
        public string Score;
        public string Description;

        public ComicConsolidatedDTO(Activity activity) : base (activity)
        {
            CategoryName = "COMIC";

            var comicActivity = activity.ComicActivity;
            var comic = comicActivity.Comic;

            Title = comic.Title;
            Person = comic.Author ?? "-";
            Type = comic.Type ?? "-";
            Series = comic.Series.Name;
            Number = comic.Number ?? "-";
            Company = comic.Publisher ?? "-";
            Year = comic.Year.ToString();
            Score = comic.Score ?? "-";
            Description = comicActivity.Description ?? "-";
        }

        public ComicConsolidatedDTO(ComicWindow comicWindow, Activity activity) : base(activity)
        {
            CategoryName = "COMIC";

            Title = comicWindow.TitleCB.Text;
            Person = comicWindow.AuthorCB.Text;
            Type = comicWindow.TypeCB.Text;
            Series = comicWindow.SeriesCB.Text;
            Number = comicWindow.NumberCB.Text;
            Company = comicWindow.PublisherCB.Text;
            Year = comicWindow.YearCB.Text;
            Score = comicWindow.ScoreCB.Text;
            Description = comicWindow.DescriptionCB.Text;
        }

        public ComicConsolidatedDTO(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "COMIC";

            Title = backupSegments[4];
            Person = backupSegments[5];
            Type = backupSegments[6];
            Series = backupSegments[7];
            Number = backupSegments[8];
            Company = backupSegments[9];
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
                + "\t" + Person + "\t" + Type
                + "\t" + Series + "\t" + Number
                + "\t" + Company + "\t" + Year
                + "\t" + Score + "\t" + Description;
        }
    }
}

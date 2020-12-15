using DomL.Business.Entities;
using DomL.Business.Utils;
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
            var comicActivity = activity.ComicActivity;
            var comic = comicActivity.Comic;

            Title = Util.GetStringOrDash(comic.Title);
            Person = Util.GetStringOrDash(comic.Person);
            Type = Util.GetStringOrDash(comic.Type);
            Series = Util.GetStringOrDash(comic.Series);
            Number = Util.GetStringOrDash(comic.Number);
            Company = Util.GetStringOrDash(comic.Company);
            Year = Util.GetStringOrDash(comic.Year);
            Score = Util.GetStringOrDash(comic.Score);
            Description = Util.GetStringOrDash(comicActivity.Description);

            FillCommonInfo();
        }

        public ComicConsolidatedDTO(ComicWindow comicWindow, Activity activity) : base(activity)
        {
            Title = Util.GetStringOrDash(comicWindow.TitleCB.Text);
            Person = Util.GetStringOrDash(comicWindow.PersonCB.Text);
            Type = Util.GetStringOrDash(comicWindow.TypeCB.Text);
            Series = Util.GetStringOrDash(comicWindow.SeriesCB.Text);
            Number = Util.GetStringOrDash(comicWindow.NumberCB.Text);
            Company = Util.GetStringOrDash(comicWindow.CompanyCB.Text);
            Year = Util.GetStringOrDash(comicWindow.YearCB.Text);
            Score = Util.GetStringOrDash(comicWindow.ScoreCB.Text);
            Description = Util.GetStringOrDash(comicWindow.DescriptionCB.Text);

            FillCommonInfo();
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

            FillCommonInfo();
        }

        private void FillCommonInfo()
        {
            CategoryName = "COMIC";
            ConsolidatedLine = GetInfoForConsolidatedLine() + "; "
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

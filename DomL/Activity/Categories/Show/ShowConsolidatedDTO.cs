using DomL.Business.Entities;
using DomL.Presentation;

namespace DomL.Business.DTOs
{
    public class ShowConsolidatedDTO : ActivityConsolidatedDTO
    {
        public string Title;
        public string Type;
        public string SeriesName;
        public string Number;
        public string Person;
        public string Company;
        public string Year;
        public string Score;
        public string Description;

        public ShowConsolidatedDTO(Activity activity) : base (activity)
        {
            CategoryName = "SHOW";

            var showActivity = activity.ShowActivity;
            var showSeason = showActivity.Show;

            Title = showSeason.Title;
            Type = showSeason.Type ?? "-";
            SeriesName = showSeason.Series.Name;
            Number = showSeason.Number;
            Person = showSeason.Person ?? "-";
            Company = showSeason.Company ?? "-";
            Year = showSeason.Year.ToString();
            Score = showSeason.Score ?? "-";
            Description = (!string.IsNullOrWhiteSpace(showActivity.Description)) ? showActivity.Description : "-";
        }

        public ShowConsolidatedDTO(ShowWindow showWindow, Activity activity) : base(activity)
        {
            CategoryName = "SHOW";

            Title = showWindow.TitleCB.Text;
            Type = showWindow.TypeCB.Text;
            SeriesName = showWindow.SeriesCB.Text;
            Number = showWindow.NumberCB.Text;
            Person = showWindow.PersonCB.Text;
            Company = showWindow.CompanyCB.Text;
            Year = showWindow.YearCB.Text;
            Score = showWindow.ScoreCB.Text;
            Description = (!string.IsNullOrWhiteSpace(showWindow.DescriptionCB.Text)) ? showWindow.DescriptionCB.Text : null;
        }

        public ShowConsolidatedDTO(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "SHOW";

            Title = backupSegments[4];
            Type = backupSegments[5];
            SeriesName = backupSegments[6];
            Number = backupSegments[7];
            Person = backupSegments[8];
            Company = backupSegments[9];
            Year = backupSegments[10];
            Score = backupSegments[11];
            Description = backupSegments[12];

            OriginalLine = GetInfoForOriginalLine() + "; "
                + GetShowActivityInfo().Replace("\t", "; ");
        }

        public new string GetInfoForYearRecap()
        {
            return base.GetInfoForYearRecap()
                + "\t" + GetShowActivityInfo();
        }

        public new string GetInfoForBackup()
        {
            return base.GetInfoForBackup()
                + "\t" + GetShowActivityInfo();
        }

        private string GetShowActivityInfo()
        {
            return Title + "\t" + Type
                + "\t" + SeriesName + "\t" + Number
                + "\t" + Person + "\t" + Company
                + "\t" + Year + "\t" + Score
                + "\t" + Description;
        }
    }
}

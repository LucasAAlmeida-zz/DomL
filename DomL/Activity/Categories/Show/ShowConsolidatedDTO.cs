using DomL.Business.Entities;
using DomL.Business.Utils;
using DomL.Presentation;

namespace DomL.Business.DTOs
{
    public class ShowConsolidatedDTO : ActivityConsolidatedDTO
    {
        public string Title;
        public string Type;
        public string Series;
        public string Number;
        public string Person;
        public string Company;
        public string Year;
        public string Score;
        public string Description;

        public ShowConsolidatedDTO(Activity activity) : base (activity)
        {
            var showActivity = activity.ShowActivity;
            var show = showActivity.Show;

            Title = Util.GetStringOrDash(show.Title);
            Type = Util.GetStringOrDash(show.Type);
            Series = Util.GetStringOrDash(show.Series);
            Number = Util.GetStringOrDash(show.Number);
            Person = Util.GetStringOrDash(show.Person);
            Company = Util.GetStringOrDash(show.Company);
            Year = Util.GetStringOrDash(show.Year);
            Score = Util.GetStringOrDash(show.Score);
            Description = Util.GetStringOrDash(showActivity.Description);

            FillCommonInfo();
        }

        public ShowConsolidatedDTO(ShowWindow showWindow, Activity activity) : base(activity)
        {
            Title = Util.GetStringOrDash(showWindow.TitleCB.Text);
            Type = Util.GetStringOrDash(showWindow.TypeCB.Text);
            Series = Util.GetStringOrDash(showWindow.SeriesCB.Text);
            Number = Util.GetStringOrDash(showWindow.NumberCB.Text);
            Person = Util.GetStringOrDash(showWindow.PersonCB.Text);
            Company = Util.GetStringOrDash(showWindow.CompanyCB.Text);
            Year = Util.GetStringOrDash(showWindow.YearCB.Text);
            Score = Util.GetStringOrDash(showWindow.ScoreCB.Text);
            Description = Util.GetStringOrDash(showWindow.DescriptionCB.Text);

            FillCommonInfo();
        }

        public ShowConsolidatedDTO(string[] backupSegments) : base(backupSegments)
        {
            Title = backupSegments[4];
            Type = backupSegments[5];
            Series = backupSegments[6];
            Number = backupSegments[7];
            Person = backupSegments[8];
            Company = backupSegments[9];
            Year = backupSegments[10];
            Score = backupSegments[11];
            Description = backupSegments[12];

            FillCommonInfo();
        }

        private void FillCommonInfo()
        {
            CategoryName = "SHOW";
            ConsolidatedLine = GetInfoForConsolidatedLine() + "; "
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
                + "\t" + Series + "\t" + Number
                + "\t" + Person + "\t" + Company
                + "\t" + Year + "\t" + Score
                + "\t" + Description;
        }
    }
}

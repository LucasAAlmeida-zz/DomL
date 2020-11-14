using DomL.Business.Entities;
using DomL.Presentation;

namespace DomL.Business.DTOs
{
    public class ConsolidatedGameDTO : ActivityConsolidatedDTO
    {
        public string Title;
        public string Platform;
        public string SeriesName;
        public string Number;
        public string Person;
        public string Company;
        public string Year;
        public string Score;
        public string Description;

        public ConsolidatedGameDTO(Activity activity) : base (activity)
        {
            var gameActivity = activity.GameActivity;
            var game = gameActivity.Game;

            Title = game.Title;
            Platform = game.Platform;
            SeriesName = (game.Series != null) ? game.Series.Name : "-";
            Number = game.Number ?? "-";
            Person = game.Person ?? "-";
            Company = game.Company ?? "-";
            Year = game.Year.ToString();
            Score = game.Score ?? "-";
            Description = (!string.IsNullOrWhiteSpace(gameActivity.Description)) ? gameActivity.Description : "-";
        }

        public ConsolidatedGameDTO(GameWindow gameWindow, Activity activity) : this(activity)
        {
            CategoryName = "Game";

            Title = gameWindow.TitleCB.Text;
            Platform = gameWindow.PlatformCB.Text;
            SeriesName = gameWindow.SeriesCB.Text;
            Number = (!string.IsNullOrWhiteSpace(gameWindow.NumberCB.Text)) ? gameWindow.NumberCB.Text : null;
            Person = gameWindow.PersonCB.Text;
            Company = gameWindow.CompanyCB.Text;
            Year = gameWindow.YearCB.Text;
            Score = gameWindow.ScoreCB.Text;
            Description = (!string.IsNullOrWhiteSpace(gameWindow.DescriptionCB.Text)) ? gameWindow.DescriptionCB.Text : null;
        }

        public ConsolidatedGameDTO(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "Game";

            Title = backupSegments[4];
            Platform = backupSegments[5];
            SeriesName = backupSegments[6];
            Number = backupSegments[7];
            Person = backupSegments[8];
            Company = backupSegments[9];
            Year = backupSegments[10];
            Score = backupSegments[11];
            Description = backupSegments[12];

            OriginalLine = GetInfoForOriginalLine() + "; "
                + GetGameActivityInfo().Replace("\t", "; ");
        }

        public new string GetInfoForYearRecap()
        {
            return base.GetInfoForYearRecap()
                + "\t" + GetGameActivityInfo();
        }

        public new string GetInfoForBackup()
        {
            return base.GetInfoForBackup()
                + "\t" + GetGameActivityInfo();
        }

        public string GetGameActivityInfo()
        {
            return Title + "\t" + Platform
                + "\t" + SeriesName + "\t" + Number
                + "\t" + Person + "\t" + Company
                + "\t" + Year + "\t" + Score
                + "\t" + Description;
        }
    }
}

using DomL.Business.Entities;
using DomL.Business.Utils;
using DomL.Presentation;

namespace DomL.Business.DTOs
{
    public class ConsolidatedGameDTO : ActivityConsolidatedDTO
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

        public ConsolidatedGameDTO(Activity activity) : base (activity)
        {
            var gameActivity = activity.GameActivity;
            var game = gameActivity.Game;

            Title = Util.GetStringOrDash(game.Title);
            Type = Util.GetStringOrDash(game.Type);
            Series = Util.GetStringOrDash(game.Series);
            Number = Util.GetStringOrDash(game.Number);
            Person = Util.GetStringOrDash(game.Person);
            Company = Util.GetStringOrDash(game.Company);
            Year = Util.GetStringOrDash(game.Year);
            Score = Util.GetStringOrDash(game.Score);
            Description = Util.GetStringOrDash(gameActivity.Description);

            FillCommonInfo();
        }

        public ConsolidatedGameDTO(GameWindow gameWindow, Activity activity) : base(activity)
        {
            Title = Util.GetStringOrDash(gameWindow.TitleCB.Text);
            Type = Util.GetStringOrDash(gameWindow.TypeCB.Text);
            Series = Util.GetStringOrDash(gameWindow.SeriesCB.Text);
            Number = Util.GetStringOrDash(gameWindow.NumberCB.Text);
            Person = Util.GetStringOrDash(gameWindow.PersonCB.Text);
            Company = Util.GetStringOrDash(gameWindow.CompanyCB.Text);
            Year = Util.GetStringOrDash(gameWindow.YearCB.Text);
            Score = Util.GetStringOrDash(gameWindow.ScoreCB.Text);
            Description = Util.GetStringOrDash(gameWindow.DescriptionCB.Text);

            FillCommonInfo();
        }

        public ConsolidatedGameDTO(string[] backupSegments) : base(backupSegments)
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
            CategoryName = "GAME";
            ConsolidatedLine = GetInfoForConsolidatedLine() + "; "
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
            return Title + "\t" + Type
                + "\t" + Series + "\t" + Number
                + "\t" + Person + "\t" + Company
                + "\t" + Year + "\t" + Score
                + "\t" + Description;
        }
    }
}

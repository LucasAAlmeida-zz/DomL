using DomL.Business.Entities;
using DomL.Presentation;

namespace DomL.Business.DTOs
{
    public class ConsolidatedGameDTO : ConsolidatedActivityDTO
    {
        public string Title;
        public string PlatformName;
        public string SeriesName;
        public string NumberInSeries;
        public string DirectorName;
        public string PublisherName;
        public string ScoreValue;
        public string Description;

        public ConsolidatedGameDTO(Activity activity) : base (activity)
        {
            var gameActivity = activity.GameActivity;
            var game = gameActivity.Game;

            Title = game.Title;
            PlatformName = game.Platform.Name;
            SeriesName = (game.Series != null) ? game.Series.Name : "-";
            NumberInSeries = game.NumberInSeries ?? "-";
            DirectorName = (game.Director != null) ? game.Director.Name : "-";
            PublisherName = (game.Publisher != null) ? game.Publisher.Name : "-";
            ScoreValue = (game.Score != null) ? game.Score.Value.ToString() : "-";
            Description = (!string.IsNullOrWhiteSpace(gameActivity.Description)) ? gameActivity.Description : "-";
        }

        public ConsolidatedGameDTO(GameWindow gameWindow, Activity activity) : this(activity)
        {
            CategoryName = "Game";

            Title = gameWindow.TitleCB.Text;
            PlatformName = gameWindow.PlatformCB.Text;
            SeriesName = gameWindow.SeriesCB.Text;
            NumberInSeries = (!string.IsNullOrWhiteSpace(gameWindow.NumberCB.Text)) ? gameWindow.NumberCB.Text : null;
            DirectorName = gameWindow.DirectorCB.Text;
            PublisherName = gameWindow.PublisherCB.Text;
            ScoreValue = gameWindow.ScoreCB.Text;
            Description = (!string.IsNullOrWhiteSpace(gameWindow.DescriptionCB.Text)) ? gameWindow.DescriptionCB.Text : null;
        }

        public ConsolidatedGameDTO(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "Game";

            Title = backupSegments[4];
            PlatformName = backupSegments[5];
            SeriesName = backupSegments[6];
            NumberInSeries = backupSegments[7];
            DirectorName = backupSegments[8];
            PublisherName = backupSegments[9];
            ScoreValue = backupSegments[10];
            Description = backupSegments[11];

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
            return Title + "\t" + PlatformName
                + "\t" + SeriesName + "\t" + NumberInSeries
                + "\t" + DirectorName + "\t" + PublisherName
                + "\t" + ScoreValue + "\t" + Description;
        }
    }
}

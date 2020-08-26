using DomL.Business.Entities;

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
        public string Score;
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
            Score = (!string.IsNullOrWhiteSpace(game.Score)) ? game.Score : "-";
            Description = (!string.IsNullOrWhiteSpace(gameActivity.Description)) ? gameActivity.Description : "-";
        }

        public string GetInfoForYearRecap()
        {
            // Date Started; Date Finished;
            // Title; Platform Name; Series Name; Number In Series; Director Name; Publisher Name; Score; Description
            return DatesStartAndFinish
                + "\t" + Title + "\t" + PlatformName
                + "\t" + SeriesName + "\t" + NumberInSeries
                + "\t" + DirectorName + "\t" + PublisherName
                + "\t" + Score + "\t" + Description;
        }
    }
}

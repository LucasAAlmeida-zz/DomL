using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class ConsolidatedShowActivityDTO : ConsolidatedActivityDTO
    {
        public string SeriesName;
        public string Season;
        public string DirectorName;
        public string Type;
        public string Score;
        public string Description;

        public ConsolidatedShowActivityDTO(Activity activity) : base (activity)
        {
            var showActivity = activity.ShowActivity;
            var showSeason = showActivity.ShowSeason;

            SeriesName = showSeason.Series.Name;
            Season = showSeason.Season;
            DirectorName = (showSeason.Director != null) ? showSeason.Director.Name : "-";
            Type = (showSeason.Type != null) ? showSeason.Type.Name : "-";
            Score = (!string.IsNullOrWhiteSpace(showSeason.Score)) ? showSeason.Score : "-";
            Description = (!string.IsNullOrWhiteSpace(showActivity.Description)) ? showActivity.Description : "-";
        }

        public string GetInfoForYearRecap()
        {
            // Date Started; Date Finished;
            // Series Name; Season; Director Name; Media Type Name; Score; Description
            return DatesStartAndFinish
                + "\t" + SeriesName + "\t" + Season
                + "\t" + DirectorName + "\t" + Type
                + "\t" + Score + "\t" + Description;
        }
    }
}

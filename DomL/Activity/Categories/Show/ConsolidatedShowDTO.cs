using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class ConsolidatedShowDTO : ConsolidatedActivityDTO
    {
        public string SeriesName;
        public string Season;
        public string DirectorName;
        public string Type;
        public string Score;
        public string Description;

        public ConsolidatedShowDTO(Activity activity) : base (activity)
        {
            var showActivity = activity.ShowActivity;
            var showSeason = showActivity.ShowSeason;

            SeriesName = showSeason.Series.Name;
            Season = showSeason.Season;
            DirectorName = (showSeason.Director != null) ? showSeason.Director.Name : "-";
            Type = (showSeason.Type != null) ? showSeason.Type.Name : "-";
            Score = (showSeason.Score != null) ? showSeason.Score.Value.ToString() : "-";
            Description = (!string.IsNullOrWhiteSpace(showActivity.Description)) ? showActivity.Description : "-";
        }

        public string GetInfoForYearRecap()
        {
            // Date Started; Date Finished;
            // Series Name; Season; Director Name; Media Type Name; Score; Description
            return DatesStartAndFinish
                + "\t" + GetShowActivityInfo();
        }

        public new string GetInfoForBackup()
        {
            return base.GetInfoForBackup()
                + "\t" + GetShowActivityInfo();
        }

        private string GetShowActivityInfo()
        {
            return SeriesName + "\t" + Season
                + "\t" + DirectorName + "\t" + Type
                + "\t" + Score + "\t" + Description;
        }
    }
}

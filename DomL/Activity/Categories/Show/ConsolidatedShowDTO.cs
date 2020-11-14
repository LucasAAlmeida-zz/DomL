using DomL.Business.Entities;
using DomL.Presentation;

namespace DomL.Business.DTOs
{
    public class ConsolidatedShowDTO : ActivityConsolidatedDTO
    {
        public string SeriesName;
        public string Season;
        public string DirectorName;
        public string TypeName;
        public string ScoreValue;
        public string Description;

        public ConsolidatedShowDTO(Activity activity) : base (activity)
        {
            CategoryName = "SHOW";

            var showActivity = activity.ShowActivity;
            var showSeason = showActivity.Show;

            SeriesName = showSeason.Series.Name;
            Season = showSeason.Season;
            DirectorName = (showSeason.Director != null) ? showSeason.Director.Name : "-";
            TypeName = (showSeason.Type != null) ? showSeason.Type.Name : "-";
            ScoreValue = (showSeason.Score != null) ? showSeason.Score.Value.ToString() : "-";
            Description = (!string.IsNullOrWhiteSpace(showActivity.Description)) ? showActivity.Description : "-";
        }

        public ConsolidatedShowDTO(ShowWindow showWindow, Activity activity) : base(activity)
        {
            CategoryName = "SHOW";

            SeriesName = showWindow.SeriesCB.Text;
            Season = showWindow.SeasonCB.Text;
            DirectorName = showWindow.DirectorCB.Text;
            TypeName = showWindow.TypeCB.Text;
            ScoreValue = showWindow.ScoreCB.Text;
            Description = (!string.IsNullOrWhiteSpace(showWindow.DescriptionCB.Text)) ? showWindow.DescriptionCB.Text : null;
        }

        public ConsolidatedShowDTO(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "SHOW";

            SeriesName = backupSegments[4];
            Season = backupSegments[5];
            DirectorName = backupSegments[6];
            TypeName = backupSegments[7];
            ScoreValue = backupSegments[8];
            Description = backupSegments[9];

            OriginalLine = GetInfoForOriginalLine()
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
            return SeriesName + "\t" + Season
                + "\t" + DirectorName + "\t" + TypeName
                + "\t" + ScoreValue + "\t" + Description;
        }
    }
}

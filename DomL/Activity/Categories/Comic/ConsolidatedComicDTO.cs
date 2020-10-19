using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class ConsolidatedComicDTO : ConsolidatedActivityDTO
    {
        public string SeriesName;
        public string Chapters;
        public string AuthorName;
        public string TypeName;
        public string ScoreValue;
        public string Description;

        public ConsolidatedComicDTO(Activity activity) : base (activity)
        {
            var comicActivity = activity.ComicActivity;
            var comicVolume = comicActivity.ComicVolume;

            SeriesName = comicVolume.Series.Name;
            Chapters = comicVolume.Chapters;
            AuthorName = (comicVolume.Author != null) ? comicVolume.Author.Name : "-";
            TypeName = (comicVolume.Type != null) ? comicVolume.Type.Name : "-";
            ScoreValue = (comicVolume.Score != null) ? comicVolume.Score.Value.ToString() : "-";
            Description = (!string.IsNullOrWhiteSpace(comicActivity.Description)) ? comicActivity.Description : "-";
        }

        public ConsolidatedComicDTO(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "COMIC";

            SeriesName = backupSegments[4];
            Chapters = backupSegments[5];
            AuthorName = backupSegments[6];
            TypeName = backupSegments[7];
            ScoreValue = backupSegments[8];
            Description = backupSegments[9];

            OriginalLine = GetInfoForOriginalLine() + "; "
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
            return SeriesName + "\t" + Chapters
                + "\t" + AuthorName + "\t" + TypeName
                + "\t" + ScoreValue + "\t" + Description;
        }
    }
}

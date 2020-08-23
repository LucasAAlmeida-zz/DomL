using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class ConsolidatedComicActivityDTO : ConsolidatedActivityDTO
    {
        public string SeriesName;
        public string Chapters;
        public string AuthorName;
        public string Type;
        public string Score;
        public string Description;

        public ConsolidatedComicActivityDTO(Activity activity) : base (activity)
        {
            var comicActivity = activity.ComicActivity;
            var comicVolume = comicActivity.ComicVolume;

            SeriesName = comicVolume.Series.Name;
            Chapters = comicVolume.Chapters;
            AuthorName = (comicVolume.Author != null) ? comicVolume.Author.Name : "-";
            Type = (comicVolume.Type != null) ? comicVolume.Type.Name : "-";
            Score = (!string.IsNullOrWhiteSpace(comicVolume.Score)) ? comicVolume.Score : "-";
            Description = comicActivity.Description;
        }

        public string GetInfoForMonthRecap()
        {
            // Date; Category; Status; Title; Series Name; Chapters; Author Name; Type; Score; Description
            return this.GetActivityInfoForMonthRecap() + "\t" + this.GetComicInfo();
        }

        public string GetInfoForYearRecap()
        {
            if (this.Status == "START" && !this.PairedDate.StartsWith("??")) {
                return "";
            }

            // Date Started; Date Finished; Title; Author Name; Series Name; Number In Series; Score; Description
            return this.GetActivityInfoForYearRecap() + "\t" + this.GetComicInfo();
        }

        public string GetInfoForBackup()
        {
            // Category; Date; DayOrder; Status; Activity Block Name;
            // Title; Author Name; Series Name; Number In Series; Score; Description
            return this.GetActivityInfoForBackup() + "\t" + this.GetComicInfo();
        }

        private string GetComicInfo()
        {
            return SeriesName + "\t" + Chapters
                + "\t" + AuthorName + "\t" + Type
                + "\t" + Score + "\t" + Description;
        }
    }
}

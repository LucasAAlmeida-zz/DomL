using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class ConsolidatedBookActivityDTO : ConsolidatedActivityDTO
    {
        public string Title;
        public string AuthorName;
        public string SeriesName;
        public string NumberInSeries;
        public string Score;
        public string Description;

        public ConsolidatedBookActivityDTO(Activity activity) : base(activity)
        {
            var bookActivity = activity.BookActivity;
            var book = bookActivity.Book;
            
            Title = book.Title;
            AuthorName = (book.Author != null) ? book.Author.Name : "-";
            SeriesName = (book.Series != null) ? book.Series.Name : "-";
            NumberInSeries = (!string.IsNullOrWhiteSpace(book.NumberInSeries)) ? book.NumberInSeries : "-";
            Score = (!string.IsNullOrWhiteSpace(book.Score)) ? book.Score : "-";
            Description = (!string.IsNullOrWhiteSpace(bookActivity.Description)) ? bookActivity.Description : "-";
        }

        public string GetInfoForMonthRecap()
        {
            // Date; Category; Status; Title; Author Name; Series Name; Number In Series; Score; Description
            return this.GetActivityInfoForMonthRecap() + "\t" + this.GetBookInfo();
        }

        public string GetInfoForYearRecap()
        {
            if (this.Status == "START" && !this.PairedDate.StartsWith("??")) {
                return "";
            }

            // Date Started; Date Finished; Title; Author Name; Series Name; Number In Series; Score; Description
            return this.GetActivityInfoForYearRecap() + "\t" + this.GetBookInfo();
        }

        public string GetInfoForBackup()
        {
            // Category; Date; DayOrder; Status; Activity Block Name;
            // Title; Author Name; Series Name; Number In Series; Score; Description
            return this.GetActivityInfoForBackup() + "\t" + this.GetBookInfo();
        }

        private string GetBookInfo()
        {
            return Title + "\t" + AuthorName
                + "\t" + SeriesName + "\t" + NumberInSeries
                + "\t" + Score + "\t" + Description;
        }
    }
}

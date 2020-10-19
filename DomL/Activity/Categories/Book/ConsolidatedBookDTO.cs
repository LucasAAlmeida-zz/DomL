using DomL.Business.Entities;
using DomL.Business.Services;
using System;

namespace DomL.Business.DTOs
{
    public class ConsolidatedBookDTO : ConsolidatedActivityDTO
    {
        public string Title;
        public string AuthorName;
        public string SeriesName;
        public string NumberInSeries;
        public string ScoreValue;
        public string Description;

        public ConsolidatedBookDTO(Activity activity) : base(activity)
        {
            var bookActivity = activity.BookActivity;
            var book = bookActivity.Book;
            
            Title = book.Title;
            AuthorName = (book.Author != null) ? book.Author.Name : "-";
            SeriesName = (book.Series != null) ? book.Series.Name : "-";
            NumberInSeries = (!string.IsNullOrWhiteSpace(book.NumberInSeries)) ? book.NumberInSeries : "-";
            ScoreValue = (book.Score != null) ? book.Score.Value.ToString() : "-";
            Description = (!string.IsNullOrWhiteSpace(bookActivity.Description)) ? bookActivity.Description : "-";
        }

        public ConsolidatedBookDTO(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "BOOK";

            Title = backupSegments[4];
            AuthorName = backupSegments[5];
            SeriesName = backupSegments[6];
            NumberInSeries = backupSegments[7];
            ScoreValue = backupSegments[8];
            Description = backupSegments[9];

            OriginalLine = GetInfoForOriginalLine() + "; "
                + GetBookActivityInfo().Replace("\t", "; ");
        }

        public new string GetInfoForYearRecap()
        {
            return base.GetInfoForYearRecap()
                + "\t" + GetBookActivityInfo();
        }

        public new string GetInfoForBackup()
        {
            return base.GetInfoForBackup()
                + "\t" + GetBookActivityInfo();
        }

        public string GetBookActivityInfo()
        {
            return Title + "\t" + AuthorName
                + "\t" + SeriesName + "\t" + NumberInSeries
                + "\t" + ScoreValue + "\t" + Description;
        }
    }
}

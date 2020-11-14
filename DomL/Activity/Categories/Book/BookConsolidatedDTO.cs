using DomL.Business.Entities;
using DomL.Presentation;

namespace DomL.Business.DTOs
{
    public class BookConsolidatedDTO : ActivityConsolidatedDTO
    {
        public string Title;
        public string Author;
        public string SeriesName;
        public string Number;
        public string Publisher;
        public string Year;
        public string Score;
        public string Description;

        public BookConsolidatedDTO(Activity activity) : base(activity)
        {
            var bookActivity = activity.BookActivity;
            var book = bookActivity.Book;
            
            Title = book.Title;
            Author = (!string.IsNullOrWhiteSpace(book.Author)) ? book.Author : "-";
            SeriesName = (book.Series != null) ? book.Series.Name : "-";
            Number = (!string.IsNullOrWhiteSpace(book.Number)) ? book.Number : "-";
            Publisher = (!string.IsNullOrWhiteSpace(book.Publisher)) ? book.Publisher : "-";
            Year = book.Year.ToString();
            Score = (!string.IsNullOrWhiteSpace(book.Score)) ? book.Score : "-";
            Description = (!string.IsNullOrWhiteSpace(bookActivity.Description)) ? bookActivity.Description : "-";
        }

        public BookConsolidatedDTO(BookWindow bookWindow, Activity activity) : base(activity)
        {
            CategoryName = "BOOK";

            Title = bookWindow.TitleCB.Text;
            Author = bookWindow.AuthorCB.Text;
            SeriesName = bookWindow.SeriesCB.Text;
            Number = (!string.IsNullOrWhiteSpace(bookWindow.NumberCB.Text)) ? bookWindow.NumberCB.Text : null;
            Publisher = bookWindow.PublisherCB.Text;
            Year = bookWindow.YearCB.Text;
            Score = bookWindow.SeriesCB.Text;
            Description = (!string.IsNullOrWhiteSpace(bookWindow.DescriptionCB.Text)) ? bookWindow.DescriptionCB.Text : null;
        }

        public BookConsolidatedDTO(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "BOOK";

            Title = backupSegments[4];
            Author = backupSegments[5];
            SeriesName = backupSegments[6];
            Number = backupSegments[7];
            Publisher = backupSegments[8];
            Year = backupSegments[9];
            Score = backupSegments[10];
            Description = backupSegments[11];

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
            return Title + "\t" + Author
                + "\t" + SeriesName + "\t" + Number
                + "\t" + Publisher + "\t" + Year
                + "\t" + Score + "\t" + Description;
        }
    }
}

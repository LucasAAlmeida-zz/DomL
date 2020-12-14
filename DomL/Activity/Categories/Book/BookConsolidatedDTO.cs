using DomL.Business.Entities;
using DomL.Presentation;

namespace DomL.Business.DTOs
{
    public class BookConsolidatedDTO : ActivityConsolidatedDTO
    {
        public string Title;
        public string Series;
        public string Number;
        public string Person;
        public string Company;
        public string Year;
        public string Score;
        public string Description;

        public BookConsolidatedDTO(Activity activity) : base(activity)
        {
            var bookActivity = activity.BookActivity;
            var book = bookActivity.Book;
            
            Title = book.Title;
            Series = (book.Series != null) ? book.Series.Name : "-";
            Number = (!string.IsNullOrWhiteSpace(book.Number)) ? book.Number : "-";
            Person = (!string.IsNullOrWhiteSpace(book.Person)) ? book.Person : "-";
            Company = (!string.IsNullOrWhiteSpace(book.Company)) ? book.Company : "-";
            Year = book.Year.ToString();
            Score = (!string.IsNullOrWhiteSpace(book.Score)) ? book.Score : "-";
            Description = (!string.IsNullOrWhiteSpace(bookActivity.Description)) ? bookActivity.Description : "-";
        }

        public BookConsolidatedDTO(BookWindow bookWindow, Activity activity) : base(activity)
        {
            CategoryName = "BOOK";

            Title = bookWindow.TitleCB.Text;
            Series = bookWindow.SeriesCB.Text;
            Number = bookWindow.NumberCB.Text;
            Person = bookWindow.PersonCB.Text;
            Company = bookWindow.CompanyCB.Text;
            Year = bookWindow.YearCB.Text;
            Score = bookWindow.SeriesCB.Text;
            Description = bookWindow.DescriptionCB.Text;
        }

        public BookConsolidatedDTO(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "BOOK";

            Title = backupSegments[4];
            Series = backupSegments[6];
            Number = backupSegments[7];
            Person = backupSegments[5];
            Company = backupSegments[8];
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
            return Title
                + "\t" + Series + "\t" + Number
                + "\t" + Person + "\t" + Company
                + "\t" + Year + "\t" + Score
                + "\t" + Description;
        }
    }
}

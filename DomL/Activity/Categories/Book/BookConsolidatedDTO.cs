using DomL.Business.Entities;
using DomL.Business.Utils;
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
            
            Title = Util.GetStringOrDash(book.Title);
            Series = Util.GetStringOrDash(book.Series);
            Number = Util.GetStringOrDash(book.Number);
            Person = Util.GetStringOrDash(book.Person);
            Company = Util.GetStringOrDash(book.Company);
            Year = Util.GetStringOrDash(book.Year);
            Score = Util.GetStringOrDash(book.Score);
            Description = Util.GetStringOrDash(bookActivity.Description);

            FillCommonInfo();
        }

        public BookConsolidatedDTO(BookWindow bookWindow, Activity activity) : base(activity)
        {
            Title = Util.GetStringOrDash(bookWindow.TitleCB.Text);
            Series = Util.GetStringOrDash(bookWindow.SeriesCB.Text);
            Number = Util.GetStringOrDash(bookWindow.NumberCB.Text);
            Person = Util.GetStringOrDash(bookWindow.PersonCB.Text);
            Company = Util.GetStringOrDash(bookWindow.CompanyCB.Text);
            Year = Util.GetStringOrDash(bookWindow.YearCB.Text);
            Score = Util.GetStringOrDash(bookWindow.SeriesCB.Text);
            Description = Util.GetStringOrDash(bookWindow.DescriptionCB.Text);

            FillCommonInfo();
        }

        public BookConsolidatedDTO(string[] backupSegments) : base(backupSegments)
        {
            Title = backupSegments[4];
            Series = backupSegments[5];
            Number = backupSegments[6];
            Person = backupSegments[7];
            Company = backupSegments[8];
            Year = backupSegments[9];
            Score = backupSegments[10];
            Description = backupSegments[11];

            FillCommonInfo();
        }

        private void FillCommonInfo()
        {
            CategoryName = "BOOK";
            ConsolidatedLine = GetInfoForConsolidatedLine() + "; "
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

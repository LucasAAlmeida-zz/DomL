using DomL.Business.Entities;
using DomL.Presentation;

namespace DomL.Business.DTOs
{
    public class CourseConsolidatedDTO : ActivityConsolidatedDTO
    {
        public string Title;
        public string Person;
        public string Type;
        public string Series;
        public string Number;
        public string Company;
        public string Year;
        public string Score;
        public string Description;

        public CourseConsolidatedDTO(Activity activity) : base (activity)
        {
            CategoryName = "COURSE";

            var courseActivity = activity.CourseActivity;
            var course = courseActivity.Course;

            Title = course.Title;
            Type = course.Type ?? "-";
            Series = course.Series;
            Number = course.Number ?? "-";
            Person = course.Person ?? "-";
            Company = course.Company ?? "-";
            Year = course.Year.ToString();
            Score = course.Score ?? "-";
            Description = courseActivity.Description ?? "-";
        }

        public CourseConsolidatedDTO(CourseWindow courseWindow, Activity activity) : base(activity)
        {
            CategoryName = "COURSE";

            Title = courseWindow.TitleCB.Text;
            Type = courseWindow.TypeCB.Text;
            Series = courseWindow.SeriesCB.Text;
            Number = courseWindow.NumberCB.Text;
            Person = courseWindow.PersonCB.Text;
            Company = courseWindow.CompanyCB.Text;
            Year = courseWindow.YearCB.Text;
            Score = courseWindow.ScoreCB.Text;
            Description = courseWindow.DescriptionCB.Text;
        }

        public CourseConsolidatedDTO(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "COURSE";

            Title = backupSegments[4];
            Type = backupSegments[6];
            Series = backupSegments[7];
            Number = backupSegments[8];
            Person = backupSegments[5];
            Company = backupSegments[9];
            Year = backupSegments[10];
            Score = backupSegments[11];
            Description = backupSegments[12];

            OriginalLine = GetInfoForOriginalLine() + "; "
                + GetCourseActivityInfo().Replace("\t", "; ");
        }

        public new string GetInfoForYearRecap()
        {
            return base.GetInfoForYearRecap()
                + "\t" + GetCourseActivityInfo();
        }

        public new string GetInfoForBackup()
        {
            return base.GetInfoForBackup()
                + "\t" + GetCourseActivityInfo();
        }

        public string GetCourseActivityInfo()
        {
            return Title + "\t" + Type
                + "\t" + Series + "\t" + Number
                + "\t" + Person + "\t" + Company
                + "\t" + Year + "\t" + Score
                + "\t" + Description;
        }
    }
}

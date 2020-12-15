using DomL.Business.Entities;
using DomL.Business.Utils;
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
            var courseActivity = activity.CourseActivity;
            var course = courseActivity.Course;

            Title = Util.GetStringOrDash(course.Title);
            Type = Util.GetStringOrDash(course.Type);
            Series = Util.GetStringOrDash(course.Series);
            Number = Util.GetStringOrDash(course.Number);
            Person = Util.GetStringOrDash(course.Person);
            Company = Util.GetStringOrDash(course.Company);
            Year = Util.GetStringOrDash(course.Year);
            Score = Util.GetStringOrDash(course.Score);
            Description = Util.GetStringOrDash(courseActivity.Description);

            FillCommonInfo();
        }

        public CourseConsolidatedDTO(CourseWindow courseWindow, Activity activity) : base(activity)
        {
            Title = Util.GetStringOrDash(courseWindow.TitleCB.Text);
            Type = Util.GetStringOrDash(courseWindow.TypeCB.Text);
            Series = Util.GetStringOrDash(courseWindow.SeriesCB.Text);
            Number = Util.GetStringOrDash(courseWindow.NumberCB.Text);
            Person = Util.GetStringOrDash(courseWindow.PersonCB.Text);
            Company = Util.GetStringOrDash(courseWindow.CompanyCB.Text);
            Year = Util.GetStringOrDash(courseWindow.YearCB.Text);
            Score = Util.GetStringOrDash(courseWindow.ScoreCB.Text);
            Description = Util.GetStringOrDash(courseWindow.DescriptionCB.Text);

            FillCommonInfo();
        }

        public CourseConsolidatedDTO(string[] backupSegments) : base(backupSegments)
        {
            Title = backupSegments[4];
            Type = backupSegments[6];
            Series = backupSegments[7];
            Number = backupSegments[8];
            Person = backupSegments[5];
            Company = backupSegments[9];
            Year = backupSegments[10];
            Score = backupSegments[11];
            Description = backupSegments[12];

            FillCommonInfo();
        }

        private void FillCommonInfo()
        {
            CategoryName = "COURSE";
            ConsolidatedLine = GetInfoForConsolidatedLine() + "; "
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

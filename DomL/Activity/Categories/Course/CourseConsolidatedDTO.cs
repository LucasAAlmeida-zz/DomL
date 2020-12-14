using DomL.Business.Entities;
using DomL.Presentation;

namespace DomL.Business.DTOs
{
    public class CourseConsolidatedDTO : ActivityConsolidatedDTO
    {
        public string Title;
        public string professor;
        public string Area;
        public string Degree;
        public string Number;
        public string School;
        public string Year;
        public string Score;
        public string Description;

        public CourseConsolidatedDTO(Activity activity) : base (activity)
        {
            CategoryName = "COURSE";

            var courseActivity = activity.CourseActivity;
            var course = courseActivity.Course;

            Title = course.Title;
            professor = course.Professor ?? "-";
            Area = course.Area ?? "-";
            Degree = course.Degree;
            Number = course.Number ?? "-";
            School = course.School ?? "-";
            Year = course.Year.ToString();
            Score = course.Score ?? "-";
            Description = courseActivity.Description ?? "-";
        }

        public CourseConsolidatedDTO(CourseWindow courseWindow, Activity activity) : base(activity)
        {
            CategoryName = "COURSE";

            Title = courseWindow.TitleCB.Text;
            professor = courseWindow.ProfessorCB.Text;
            Area = courseWindow.AreaCB.Text;
            Degree = courseWindow.DegreeCB.Text;
            Number = courseWindow.NumberCB.Text;
            School = courseWindow.SchoolCB.Text;
            Year = courseWindow.YearCB.Text;
            Score = courseWindow.ScoreCB.Text;
            Description = courseWindow.DescriptionCB.Text;
        }

        public CourseConsolidatedDTO(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "COURSE";

            Title = backupSegments[4];
            professor = backupSegments[5];
            Area = backupSegments[6];
            Degree = backupSegments[7];
            Number = backupSegments[8];
            School = backupSegments[9];
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
            return Title 
                + "\t" + professor + "\t" + Area
                + "\t" + Degree + "\t" + Number
                + "\t" + School + "\t" + Year
                + "\t" + Score + "\t" + Description;
        }
    }
}

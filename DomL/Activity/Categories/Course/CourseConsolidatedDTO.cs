using DomL.Business.Entities;
using DomL.Presentation;

namespace DomL.Business.DTOs
{
    public class CourseConsolidatedDTO : ActivityConsolidatedDTO
    {
        public string Name;
        public string School;
        public string Teacher;
        public string Year;
        public string Score;
        public string Description;

        public CourseConsolidatedDTO(Activity activity) : base (activity)
        {
            CategoryName = "COURSE";

            var courseActivity = activity.CourseActivity;
            var course = courseActivity.Course;

            Name = course.Name;
            School = course.School ?? "-";
            Teacher = course.Teacher ?? "-";
            Score = course.Score ?? "-";
            Year = course.Year.ToString();
            Description = (!string.IsNullOrWhiteSpace(courseActivity.Description)) ? courseActivity.Description : "-";
        }

        public CourseConsolidatedDTO(CourseWindow courseWindow, Activity activity) : base(activity)
        {
            CategoryName = "COURSE";

            Name = courseWindow.NameCB.Text;
            Teacher = courseWindow.TeacherCB.Text;
            School = courseWindow.SchoolCB.Text;
            Year = courseWindow.YearCB.Text;
            Score = courseWindow.ScoreCB.Text;
            Description = (!string.IsNullOrWhiteSpace(courseWindow.DescriptionCB.Text)) ? courseWindow.DescriptionCB.Text : null;
        }

        public CourseConsolidatedDTO(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "COURSE";

            Name = backupSegments[4];
            School = backupSegments[5];
            Teacher = backupSegments[6];
            Year = backupSegments[7];
            Score = backupSegments[8];
            Description = backupSegments[9];

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
            return Name 
                + "\t" + School + "\t" + Teacher
                + "\t" + Year + "\t" + Score
                + "\t" + Description;
        }
    }
}

using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class ConsolidatedCourseDTO : ConsolidatedActivityDTO
    {
        public string Name;
        public string SchoolName;
        public string TeacherName;
        public string Score;
        public string Description;

        public ConsolidatedCourseDTO(Activity activity) : base (activity)
        {
            var courseActivity = activity.CourseActivity;
            var course = courseActivity.Course;

            Name = course.Name;
            SchoolName = (course.School != null) ? course.School.Name : "-";
            TeacherName = (course.Teacher != null) ? course.Teacher.Name : "-";
            Score = (course.Score != null) ? course.Score.Value.ToString() : "-";
            Description = (!string.IsNullOrWhiteSpace(courseActivity.Description)) ? courseActivity.Description : "-";
        }

        public string GetInfoForYearRecap()
        {
            // Date Started; Date Finished;
            // Name; School Name; Teacher Name; Score; Description
            return DatesStartAndFinish
                + "\t" + GetCourseActivityInfo();
        }

        public new string GetInfoForBackup()
        {
            return base.GetInfoForBackup()
                + "\t" + GetCourseActivityInfo();
        }

        public string GetCourseActivityInfo()
        {
            return Name + "\t" + SchoolName + "\t" + TeacherName
                + "\t" + Score + "\t" + Description;
        }
    }
}

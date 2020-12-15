using DomL.Business.Entities;
using DomL.Business.Utils;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

namespace DomL.DataAccess
{
    public class CourseRepository : DomLRepository<CourseActivity>
    {
        public CourseRepository(DomLContext context) : base(context) { }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }

        public Course GetCourseByName(string name)
        {
            var cleanName = Util.CleanString(name);
            return DomLContext.Course
                .SingleOrDefault(u =>
                    u.Title.Replace(":", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").Replace("'", "").Replace(",", "").ToLower().Replace("the", "")
                    == cleanName
                );
        }

        public void CreateCourseActivity(CourseActivity courseActivity)
        {
            DomLContext.CourseActivity.Add(courseActivity);
        }

        public void CreateCourse(Course course)
        {
            DomLContext.Course.Add(course);
        }

        public List<Course> GetAllCourses()
        {
            return DomLContext.Course.ToList();
        }
    }
}

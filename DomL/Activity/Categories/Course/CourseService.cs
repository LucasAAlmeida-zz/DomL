using DomL.Business.DTOs;
using DomL.Business.Entities;
using DomL.DataAccess;
using DomL.Presentation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DomL.Business.Services
{
    public class CourseService
    {
        public static void SaveFromRawSegments(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            segments[0] = "";
            var courseWindow = new CourseWindow(segments, activity, unitOfWork);

            if (ConfigurationManager.AppSettings["ShowCategoryWindows"] == "true") {
                courseWindow.ShowDialog();
            }

            var consolidated = new ConsolidatedCourseDTO(courseWindow, activity);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        public static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            var consolidated = new ConsolidatedCourseDTO(backupSegments);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        private static void SaveFromConsolidated(ConsolidatedCourseDTO consolidated, UnitOfWork unitOfWork)
        {
            var teacher = PersonService.GetOrCreateByName(consolidated.TeacherName, unitOfWork);
            var company = CompanyService.GetOrCreateByName(consolidated.SchoolName, unitOfWork);
            var score = ScoreService.GetByValue(consolidated.ScoreValue, unitOfWork);

            var course = GetOrUpdateOrCreateCourse(consolidated.Name, teacher, company, score, unitOfWork);

            var activity = ActivityService.Create(consolidated, unitOfWork);
            CreateCourseActivity(activity, course, consolidated.Description, unitOfWork);
        }

        private static Course GetOrUpdateOrCreateCourse(string name, Person teacher, Company school, Score score, UnitOfWork unitOfWork)
        {
            var course = GetCourseByName(name, unitOfWork);

            if (course == null) {
                course = new Course() {
                    Name = name,
                    Teacher = teacher,
                    School = school,
                    Score = score,
                };
                unitOfWork.CourseRepo.CreateCourse(course);
            } else {
                course.Teacher = teacher ?? course.Teacher;
                course.School = school ?? course.School;
                course.Score = score ?? course.Score;
            }

            return course;
        }

        private static void CreateCourseActivity(Activity activity, Course course, string description, UnitOfWork unitOfWork)
        {
            var courseActivity = new CourseActivity() {
                Activity = activity,
                Course = course,
                Description = description
            };

            activity.CourseActivity = courseActivity;
            ActivityService.PairUpWithStartingActivity(activity, unitOfWork);

            unitOfWork.CourseRepo.CreateCourseActivity(courseActivity);
        }

        public static List<Course> GetAll(UnitOfWork unitOfWork)
        {
            return unitOfWork.CourseRepo.GetAllCourses().ToList();
        }

        public static IEnumerable<Activity> GetStartingActivities(IQueryable<Activity> previousStartingActivities, Activity activity)
        {
            var course = activity.CourseActivity.Course;
            return previousStartingActivities.Where(u => 
                u.CategoryId == ActivityCategory.COURSE_ID
                && u.CourseActivity.Course.Name == course.Name
            );
        }

        public static Course GetCourseByName(string name, UnitOfWork unitOfWork)
        {
            return unitOfWork.CourseRepo.GetCourseByName(name);
        }
    }
}

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

            var consolidated = new CourseConsolidatedDTO(courseWindow, activity);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        public static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            var consolidated = new CourseConsolidatedDTO(backupSegments);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        private static void SaveFromConsolidated(CourseConsolidatedDTO consolidated, UnitOfWork unitOfWork)
        {
            var course = GetOrUpdateOrCreateCourse(consolidated, unitOfWork);
            var activity = ActivityService.Create(consolidated, unitOfWork);
            CreateCourseActivity(activity, course, consolidated.Description, unitOfWork);
        }

        private static Course GetOrUpdateOrCreateCourse(CourseConsolidatedDTO consolidated, UnitOfWork unitOfWork)
        {
            var course = GetCourseByName(consolidated.Name, unitOfWork);

            if (course == null) {
                course = new Course() {
                    Name = consolidated.Name,
                    Teacher = consolidated.Teacher,
                    School = consolidated.School,
                    Score = consolidated.Score,
                };
                unitOfWork.CourseRepo.CreateCourse(course);
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

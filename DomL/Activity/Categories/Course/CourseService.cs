using DomL.Business.DTOs;
using DomL.Business.Entities;
using DomL.Business.Utils;
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
            var course = GetCourseByName(consolidated.Title, unitOfWork);

            if (course == null) {
                course = new Course() {
                    Title = Util.GetStringOrNull(consolidated.Title),
                    Professor = Util.GetStringOrNull(consolidated.Professor),
                    Area = Util.GetStringOrNull(consolidated.Area),
                    Degree = Util.GetStringOrNull(consolidated.Degree),
                    Number = Util.GetStringOrNull(consolidated.Number),
                    School = Util.GetStringOrNull(consolidated.School),
                    Year = Util.GetIntOrZero(consolidated.Year),
                    Score = Util.GetStringOrNull(consolidated.Score),
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
                && u.CourseActivity.Course.Title == course.Title
            );
        }

        public static Course GetCourseByName(string name, UnitOfWork unitOfWork)
        {
            return unitOfWork.CourseRepo.GetCourseByName(name);
        }
    }
}

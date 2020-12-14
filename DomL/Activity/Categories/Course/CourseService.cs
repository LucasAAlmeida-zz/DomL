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
            var instance = GetByName(consolidated.Title, unitOfWork);

            var title = Util.GetStringOrNull(consolidated.Title);
            var area = Util.GetStringOrNull(consolidated.Area);
            var degree = Util.GetStringOrNull(consolidated.Degree);
            var number = Util.GetStringOrNull(consolidated.Number);
            var professor = Util.GetStringOrNull(consolidated.professor);
            var school = Util.GetStringOrNull(consolidated.School);
            var year = Util.GetIntOrZero(consolidated.Year);
            var score = Util.GetStringOrNull(consolidated.Score);

            if (instance == null) {
                instance = new Course() {
                    Title = title,
                    Area = area,
                    Degree = degree,
                    Number = number,
                    Professor = professor,
                    School = school,
                    Year = year,
                    Score = score,
                };
            } else {
                instance.Area = area ?? instance.Area;
                instance.Degree = degree ?? instance.Degree;
                instance.Number = number ?? instance.Number;
                instance.Professor = professor ?? instance.Professor;
                instance.School = school ?? instance.School;
                instance.Year = year != 0 ? year : instance.Year;
                instance.Score = score ?? instance.Score;
            }

            return instance;
        }

        private static void CreateCourseActivity(Activity activity, Course course, string description, UnitOfWork unitOfWork)
        {
            var courseActivity = new CourseActivity() {
                Activity = activity,
                Course = course,
                Description = Util.GetStringOrNull(description)
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
                u.CategoryId == Category.COURSE_ID
                && u.CourseActivity.Course.Title == course.Title
            );
        }

        public static Course GetByName(string name, UnitOfWork unitOfWork)
        {
            return unitOfWork.CourseRepo.GetCourseByName(name);
        }
    }
}

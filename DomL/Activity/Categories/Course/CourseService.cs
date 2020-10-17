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
            // COURSE (Classification); Name; (School Name); (Teacher Name); (Score); (Description)
            segments[0] = "";
            var courseWindow = new CourseWindow(segments, activity, unitOfWork);

            if (ConfigurationManager.AppSettings["ShowCategoryWindows"] == "true") {
                courseWindow.ShowDialog();
            }

            var name = courseWindow.NameCB.Text;
            var teacherName = courseWindow.TeacherCB.Text;
            var schoolName = courseWindow.SchoolCB.Text;
            var scoreValue = courseWindow.ScoreCB.Text;
            var description = (!string.IsNullOrWhiteSpace(courseWindow.DescriptionCB.Text)) ? courseWindow.DescriptionCB.Text : null;

            var teacher = PersonService.GetOrCreateByName(teacherName, unitOfWork);
            var company = CompanyService.GetOrCreateByName(schoolName, unitOfWork);
            var score = ScoreService.GetByValue(scoreValue, unitOfWork);

            Course course = GetOrUpdateOrCreateCourse(name, teacher, company, score, unitOfWork);
            CreateCourseActivity(activity, course, description, unitOfWork);
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

        public static void RestoreFromFile(string fileDir)
        {
            using (var reader = new StreamReader(fileDir + "Course.txt")) {
                string line = "";
                while ((line = reader.ReadLine()) != null) {
                    if (string.IsNullOrWhiteSpace(line)) {
                        continue;
                    }

                    var segments = Regex.Split(line, "\t");

                    // Date Start; Date Finish; Name; (School Name); (Teacher Name); (Score); (Description)
                    var dateStart = segments[0];
                    var dateFinish = segments[1];
                    var name = segments[2];
                    var schoolName = segments[3] != "-" ? segments[3] : null;
                    var teacherName = segments[4] != "-" ? segments[4] : null;
                    var scoreValue = segments[5] != "-" ? segments[5] : null;
                    var description = segments[6] != "-" ? segments[6] : null;

                    var originalLine = name;
                    originalLine = (!string.IsNullOrWhiteSpace(schoolName)) ? originalLine + "; " + schoolName : originalLine;
                    originalLine = (!string.IsNullOrWhiteSpace(teacherName)) ? originalLine + "; " + teacherName : originalLine;
                    originalLine = (!string.IsNullOrWhiteSpace(scoreValue)) ? originalLine + "; " + scoreValue : originalLine;
                    originalLine = (!string.IsNullOrWhiteSpace(description)) ? originalLine + "; " + description : originalLine;

                    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                        var teacher = PersonService.GetByName(teacherName, unitOfWork);
                        var company = CompanyService.GetOrCreateByName(schoolName, unitOfWork);
                        var score = ScoreService.GetByValue(scoreValue, unitOfWork);

                        var course = GetOrUpdateOrCreateCourse(name, teacher, company, score, unitOfWork);

                        var statusStart = unitOfWork.ActivityRepo.GetStatusById(ActivityStatus.START);
                        var statusFinish = unitOfWork.ActivityRepo.GetStatusById(ActivityStatus.FINISH);
                        var statusSingle = unitOfWork.ActivityRepo.GetStatusById(ActivityStatus.SINGLE);

                        var category = unitOfWork.ActivityRepo.GetCategoryById(ActivityCategory.COURSE_ID);

                        if (!dateStart.StartsWith("--") && dateStart != dateFinish) {
                            Activity activityStart = null;
                            Activity activityFinish = null;
                            if (!dateStart.StartsWith("??")) {
                                var date = DateTime.ParseExact(dateStart, "dd/MM/yy", null);
                                activityStart = ActivityService.Create(date, 0, statusStart, category, null, "COURSE Start; " + originalLine, unitOfWork);
                                CreateCourseActivity(activityStart, course, description, unitOfWork);
                            }
                            if (!dateFinish.StartsWith("??")) {
                                var date = DateTime.ParseExact(dateFinish, "dd/MM/yy", null);
                                activityFinish = ActivityService.Create(date, 0, statusFinish, category, null, "COURSE Finish; " + originalLine, unitOfWork);
                                CreateCourseActivity(activityFinish, course, description, unitOfWork);
                            }

                            if (activityStart != null && activityFinish != null) {
                                activityStart.CourseActivity.Description = null;
                                activityStart.PairedActivity = activityFinish;
                                unitOfWork.Complete();
                                activityFinish.PairedActivity = activityStart;
                            }
                        } else {
                            var date = DateTime.ParseExact(dateFinish, "dd/MM/yy", null);
                            var activity = ActivityService.Create(date, 0, statusSingle, category, null, "COURSE; " + originalLine, unitOfWork);
                            CreateCourseActivity(activity, course, description, unitOfWork);
                        }

                        unitOfWork.Complete();
                    }
                }
            }
        }
    }
}

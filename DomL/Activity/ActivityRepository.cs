using DomL.Business.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DomL.DataAccess.Repositories
{
    public class ActivityRepository : DomLRepository<Activity>
    {
        public ActivityRepository(DomLContext context) : base(context) { }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }

        public IQueryable<Activity> GetPreviousStartingActivities(DateTime Date)
        {
            return GetAllQueryableInclusive()
                .Where(u =>
                    u.Status.Id == ActivityStatus.START
                    && u.Date <= Date
                    && u.PairedActivityId == null
                );
        }

        public List<Activity> GetAllInclusiveFromMonth(int month, int year)
        {
            return GetAllQueryableInclusive()
                .Where(u => u.Date.Month == month && u.Date.Year == year)
                .ToList();
        }

        public List<Activity> GetAllInclusiveFromYear(int year)
        {
            return GetAllQueryableInclusive()
                .Where(u => u.Date.Year == year)
                .ToList();
        }

        public List<Activity> GetAllInclusiveFromCategory(int categoryId)
        {
            return GetAllQueryableInclusive()
                .Where(u => u.CategoryId == categoryId)
                .ToList();
        }

        private IOrderedQueryable<Activity> GetAllQueryableInclusive()
        {
            return DomLContext.Activity
                .Include(u => u.Category)
                .Include(u => u.Status)
                .Include(u => u.ActivityBlock)
                .Include(u => u.AutoActivity)
                .Include(u => u.BookActivity.Book.Series)
                .Include(u => u.ComicActivity.Comic.Series)
                .Include(u => u.CourseActivity.Course)
                .Include(u => u.DoomActivity)
                .Include(u => u.EventActivity)
                .Include(u => u.GameActivity.Game.Series)
                .Include(u => u.GiftActivity)
                .Include(u => u.HealthActivity.Specialty)
                .Include(u => u.MovieActivity.Movie.Series)
                .Include(u => u.PetActivity.Pet)
                .Include(u => u.MeetActivity)
                .Include(u => u.PlayActivity)
                .Include(u => u.PurchaseActivity.Store)
                .Include(u => u.ShowActivity.Show.Series)
                .Include(u => u.TravelActivity)
                .Include(u => u.WorkActivity.Work)
                .OrderBy(a => a.Date).ThenBy(a => a.DayOrder);
        }

        public void DeleteAllFromMonth(int month, int year)
        {
            DomLContext.Activity
                .Where(u=> u.PairedActivityId != null &&
                    (
                        (u.Date.Month == month && u.Date.Year == year) 
                        ||
                        (u.PairedActivity.Date.Month == month && u.PairedActivity.Date.Year == year)
                    )
                )
                .ToList()
                .ForEach(u => u.PairedActivityId = null);
            DomLContext.SaveChanges();

            DomLContext.Activity.RemoveRange(
                DomLContext.Activity
                    .Include(u => u.AutoActivity)
                    .Include(u => u.BookActivity)
                    .Include(u => u.ComicActivity)
                    .Include(u => u.CourseActivity)
                    .Include(u => u.DoomActivity)
                    .Include(u => u.EventActivity)
                    .Include(u => u.GameActivity)
                    .Include(u => u.GiftActivity)
                    .Include(u => u.HealthActivity)
                    .Include(u => u.MovieActivity)
                    .Include(u => u.PetActivity)
                    .Include(u => u.MeetActivity)
                    .Include(u => u.PlayActivity)
                    .Include(u => u.PurchaseActivity)
                    .Include(u => u.ShowActivity)
                    .Include(u => u.TravelActivity)
                    .Include(u => u.WorkActivity)
                    .Where(u => u.Date.Month == month && u.Date.Year == year)
            );
        }

        public void DeleteAllFromCategory(int categoryId)
        {
            DomLContext.Activity
                .Where(u => 
                    u.PairedActivityId != null && 
                    (u.CategoryId == categoryId || u.PairedActivity.CategoryId == categoryId)
                )
                .ToList()
                .ForEach(u => u.PairedActivityId = null);
            DomLContext.SaveChanges();

            DomLContext.Activity.RemoveRange(
                DomLContext.Activity
                    .Include(u => u.AutoActivity)
                    .Include(u => u.BookActivity)
                    .Include(u => u.ComicActivity)
                    .Include(u => u.CourseActivity)
                    .Include(u => u.DoomActivity)
                    .Include(u => u.EventActivity)
                    .Include(u => u.GameActivity)
                    .Include(u => u.GiftActivity)
                    .Include(u => u.HealthActivity)
                    .Include(u => u.MovieActivity)
                    .Include(u => u.PetActivity)
                    .Include(u => u.MeetActivity)
                    .Include(u => u.PlayActivity)
                    .Include(u => u.PurchaseActivity)
                    .Include(u => u.ShowActivity)
                    .Include(u => u.TravelActivity)
                    .Include(u => u.WorkActivity)
                    .Where(u => u.CategoryId == categoryId)
            );
        }

        public void CreateActivityBlock(ActivityBlock activityBlock)
        {
            DomLContext.ActivityBlock.Add(activityBlock);
        }

        public ActivityBlock GetActivityBlockByName(string blockName)
        {
            return DomLContext.ActivityBlock.SingleOrDefault(u => u.Name == blockName);
        }

        public ActivityCategory GetCategoryByName(string categoryName)
        {
            return DomLContext.ActivityCategory.SingleOrDefault(u => u.Name == categoryName);
        }

        public ActivityCategory GetCategoryById(int id)
        {
            return DomLContext.ActivityCategory.SingleOrDefault(u => u.Id == id);
        }

        public List<ActivityCategory> GetAllCategories()
        {
            return DomLContext.ActivityCategory.ToList();
        }
        
        public ActivityStatus GetStatusByName(string statusName)
        {
            return DomLContext.ActivityStatus.SingleOrDefault(u => u.Name == statusName);
        }

        public ActivityStatus GetStatusById(int id)
        {
            return DomLContext.ActivityStatus.SingleOrDefault(u => u.Id == id);
        }
    }
}

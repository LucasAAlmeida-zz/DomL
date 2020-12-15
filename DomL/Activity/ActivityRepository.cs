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
                    u.Status.Id == Status.START
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
                .Include(u => u.AutoActivity)
                .Include(u => u.BookActivity.Book)
                .Include(u => u.ComicActivity.Comic)
                .Include(u => u.CourseActivity.Course)
                .Include(u => u.DoomActivity)
                .Include(u => u.EventActivity)
                .Include(u => u.GameActivity.Game)
                .Include(u => u.GiftActivity)
                .Include(u => u.HealthActivity)
                .Include(u => u.MovieActivity.Movie)
                .Include(u => u.PetActivity)
                .Include(u => u.MeetActivity)
                .Include(u => u.PlayActivity)
                .Include(u => u.PurchaseActivity)
                .Include(u => u.ShowActivity.Show)
                .Include(u => u.TravelActivity)
                .Include(u => u.WorkActivity)
                .OrderBy(a => a.Date).ThenBy(a => a.DayOrder);
        }

        public void DeleteAllFromDay(DateTime date)
        {
            DomLContext.Activity
                .Where(u => u.PairedActivityId != null && (u.Date == date || u.PairedActivity.Date == date))
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
                    .Where(u => u.Date == date)
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

        public Category GetCategoryByName(string categoryName)
        {
            return DomLContext.ActivityCategory.SingleOrDefault(u => u.Name == categoryName);
        }

        public Category GetCategoryById(int id)
        {
            return DomLContext.ActivityCategory.SingleOrDefault(u => u.Id == id);
        }

        public List<Category> GetAllCategories()
        {
            return DomLContext.ActivityCategory.ToList();
        }
        
        public Status GetStatusByName(string statusName)
        {
            return DomLContext.ActivityStatus.SingleOrDefault(u => u.Name == statusName);
        }

        public Status GetStatusById(int id)
        {
            return DomLContext.ActivityStatus.SingleOrDefault(u => u.Id == id);
        }
    }
}

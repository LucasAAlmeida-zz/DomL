using DomL.Business.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DomL.DataAccess.Repositories
{
    public class ActivityRepository : BaseRepository<Activity>
    {
        public ActivityRepository(DomLContext context) : base(context) { }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
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

        public List<Activity> GetAllInclusive()
        {
            return GetAllQueryableInclusive()
                .ToList();
        }

        private IOrderedQueryable<Activity> GetAllQueryableInclusive()
        {
            return DomLContext.Activity
                .Include(u => u.Category)
                .Include(u => u.Status)
                .Include(u => u.AutoActivity.Auto)
                .Include(u => u.BookActivity.Book.Author).Include(u => u.BookActivity.Book.Series)
                .Include(u => u.ComicActivity.ComicVolume.Author).Include(u => u.ComicActivity.ComicVolume.Series).Include(u => u.ComicActivity.ComicVolume.Type)
                .Include(u => u.DoomActivity)
                .Include(u => u.EventActivity)
                .Include(u => u.GameActivity.Game.Platform).Include(u => u.GameActivity.Game.Series).Include(u => u.GameActivity.Game.Director).Include(u => u.GameActivity.Game.Publisher)
                .Include(u => u.GiftActivity.Person)
                .Include(u => u.HealthActivity.Specialty)
                .Include(u => u.MovieActivity.Movie.Director).Include(u => u.MovieActivity.Movie.Series)
                .Include(u => u.PetActivity.Pet)
                .Include(u => u.MeetActivity.Person)
                .Include(u => u.PlayActivity.Person)
                .Include(u => u.PurchaseActivity.Store)
                .Include(u => u.ShowActivity.ShowSeason.Director).Include(u => u.ShowActivity.ShowSeason.Series).Include(u => u.ShowActivity.ShowSeason.Type)
                .Include(u => u.TravelActivity.Transport).Include(u => u.TravelActivity.Origin).Include(u => u.TravelActivity.Destination)
                .OrderBy(a => a.Date).ThenBy(a => a.DayOrder);
        }

        public void DeleteAllFromMonth(int month, int year)
        {
            DomLContext.Activity.Where(u=> u.Date.Month == month && u.Date.Year == year && u.PairedActivityId != null).ToList().ForEach(u => u.PairedActivityId = null);
            DomLContext.SaveChanges();
            DomLContext.Activity.RemoveRange(
                DomLContext.Activity
                    .Include(u => u.AutoActivity)
                    .Include(u => u.BookActivity)
                    .Include(u => u.ComicActivity)
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
                    .Where(u => u.Date.Month == month && u.Date.Year == year)
            );
        }

        public ActivityCategory GetCategoryByName(string categoryName)
        {
            return DomLContext.ActivityCategory.SingleOrDefault(u => u.Name == categoryName);
        }

        public ActivityStatus GetStatusByName(string statusName)
        {
            return DomLContext.ActivityStatus.SingleOrDefault(u => u.Name == statusName);
        }

        public void CreateActivityBlock(ActivityBlock activityBlock)
        {
            DomLContext.ActivityBlock.Add(activityBlock);
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

        public List<ActivityCategory> GetAllCategories()
        {
            return DomLContext.ActivityCategory.ToList();
        }
    }
}

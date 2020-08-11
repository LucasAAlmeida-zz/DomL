using DomL.Business;
using DomL.Business.Activities;
using DomL.Business.Activities.MultipleDayActivities;
using DomL.Business.Activities.SingleDayActivities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;

namespace DomL.DataAccess
{
    public class ActivityBlockRepository : BaseRepository<ActivityBlock>
    {
        public ActivityBlockRepository(DomLContext context)
        : base(context)
        {
        }

        public List<Activity> GetActivitiesInBlockYear(int year)
        {
            //using (var unitOfWork = new UnitOfWork(new DomLContext())) {
            //    return unitOfWork.BookRepo.Find(b => b.Date.Year == ano);
            //}

            var activitiesInBlock = new List<Activity>();

            activitiesInBlock.AddRange(DomLContext.Book.Where(b => b.Date.Year == year && b.ActivityBlockId != null));
            activitiesInBlock.AddRange(DomLContext.Comic.Where(b => b.Date.Year == year && b.ActivityBlockId != null));
            activitiesInBlock.AddRange(DomLContext.Game.Where(b => b.Date.Year == year && b.ActivityBlockId != null));
            activitiesInBlock.AddRange(DomLContext.Series.Where(b => b.Date.Year == year && b.ActivityBlockId != null));
            activitiesInBlock.AddRange(DomLContext.Watch.Where(b => b.Date.Year == year && b.ActivityBlockId != null));
            activitiesInBlock.AddRange(DomLContext.Auto.Where(b => b.Date.Year == year && b.ActivityBlockId != null));
            activitiesInBlock.AddRange(DomLContext.Doom.Where(b => b.Date.Year == year && b.ActivityBlockId != null));
            activitiesInBlock.AddRange(DomLContext.Gift.Where(b => b.Date.Year == year && b.ActivityBlockId != null));
            activitiesInBlock.AddRange(DomLContext.Health.Where(b => b.Date.Year == year && b.ActivityBlockId != null));
            activitiesInBlock.AddRange(DomLContext.Movie.Where(b => b.Date.Year == year && b.ActivityBlockId != null));
            activitiesInBlock.AddRange(DomLContext.Person.Where(b => b.Date.Year == year && b.ActivityBlockId != null));
            activitiesInBlock.AddRange(DomLContext.Pet.Where(b => b.Date.Year == year && b.ActivityBlockId != null));
            activitiesInBlock.AddRange(DomLContext.Play.Where(b => b.Date.Year == year && b.ActivityBlockId != null));
            activitiesInBlock.AddRange(DomLContext.Purchase.Where(b => b.Date.Year == year && b.ActivityBlockId != null));
            activitiesInBlock.AddRange(DomLContext.Travel.Where(b => b.Date.Year == year && b.ActivityBlockId != null));
            activitiesInBlock.AddRange(DomLContext.Work.Where(b => b.Date.Year == year && b.ActivityBlockId != null));
            activitiesInBlock.AddRange(DomLContext.Event.Where(b => b.Date.Year == year && b.ActivityBlockId != null));

            return activitiesInBlock.OrderBy(aib => aib.Date).ToList();
        }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }

    }
}

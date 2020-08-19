using DomL.Business.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomL.DataAccess.Repositories
{
    public class ActivityRepository : BaseRepository<Activity>
    {
        public ActivityRepository(DomLContext context) : base(context) { }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }

        public ActivityBlock CreateActivityBlock(ActivityBlock activityBlock)
        {
            activityBlock = DomLContext.ActivityBlock.Add(activityBlock);
            DomLContext.SaveChanges();
            return activityBlock;
        }

        public Activity CreateActivity(Activity activity)
        {
            activity = DomLContext.Activity.Add(activity);
            DomLContext.SaveChanges();
            return activity;
        }
    }
}

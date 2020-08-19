using DomL.Business.Entities;
using System;
using System.Linq;

namespace DomL.DataAccess
{
    public class AutoRepository : BaseRepository<Auto>
    {
        public AutoRepository(DomLContext context) : base(context) { }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }

        public Auto GetAutoByName(string name)
        {
            return DomLContext.Auto.SingleOrDefault(a => a.Name == name);
        }

        public Auto CreateAuto(Auto auto)
        {
            auto = DomLContext.Auto.Add(auto);
            DomLContext.SaveChanges();
            return auto;
        }

        public AutoActivity CreateAutoActivity(AutoActivity autoActivity)
        {
            autoActivity = DomLContext.AutoActivity.Add(autoActivity);
            DomLContext.SaveChanges();
            return autoActivity;
        }
    }
}

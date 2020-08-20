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

        public void CreateAuto(Auto auto)
        {
            DomLContext.Auto.Add(auto);
        }

        public void CreateAutoActivity(AutoActivity autoActivity)
        {
            DomLContext.AutoActivity.Add(autoActivity);
        }
    }
}

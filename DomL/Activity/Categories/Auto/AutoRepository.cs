using DomL.Business.Entities;
using System;
using System.Linq;

namespace DomL.DataAccess
{
    public class AutoRepository : DomLRepository<AutoActivity>
    {
        public AutoRepository(DomLContext context) : base(context) { }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }

        public void CreateAutoActivity(AutoActivity autoActivity)
        {
            DomLContext.AutoActivity.Add(autoActivity);
        }
    }
}

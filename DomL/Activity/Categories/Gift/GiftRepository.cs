using DomL.Business.Entities;
using System;

namespace DomL.DataAccess
{
    public class GiftRepository : DomLRepository<GiftActivity>
    {
        public GiftRepository(DomLContext context) : base(context) { }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }

        public void CreateGiftActivity(GiftActivity giftActivity)
        {
            DomLContext.GiftActivity.Add(giftActivity);
        }
    }
}

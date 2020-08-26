using DomL.Business.Entities;
using System;
using System.Linq;

namespace DomL.DataAccess
{
    public class PurchaseRepository : BaseRepository<PurchaseActivity>
    {
        public PurchaseRepository(DomLContext context) : base(context) { }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }

        public void CreatePurchaseActivity(PurchaseActivity purchaseActivity)
        {
            DomLContext.PurchaseActivity.Add(purchaseActivity);
        }
    }
}


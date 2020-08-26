using DomL.Business.DTOs;
using DomL.Business.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DomL.Business.Services
{
    public class PurchaseService
    {
        public static void SaveFromRawSegments(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            // PURCHASE; Store Name; Product; Value; (Description)
            var storeName = segments[1];
            var product = segments[2];
            var value = int.Parse(segments[3]);
            var description = (segments.Length > 4) ? segments[4] : null;

            Company store = CompanyService.GetOrCreateByName(storeName, unitOfWork);

            CreatePurchaseActivity(activity, store, product, value, description, unitOfWork);
        }

        private static void CreatePurchaseActivity(Activity activity, Company store, string product, int value, string description, UnitOfWork unitOfWork)
        {
            var purchaseActivity = new PurchaseActivity() {
                Activity = activity,
                Store = store,
                Product = product,
                Value = value,
                Description = description
            };

            activity.PurchaseActivity = purchaseActivity;

            unitOfWork.PurchaseRepo.CreatePurchaseActivity(purchaseActivity);
        }
    }
}

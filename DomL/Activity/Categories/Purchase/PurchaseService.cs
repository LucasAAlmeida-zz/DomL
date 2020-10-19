using DomL.Business.DTOs;
using DomL.Business.Entities;
using DomL.DataAccess;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace DomL.Business.Services
{
    public class PurchaseService
    {
        public static void SaveFromRawSegments(string[] rawSegments, Activity activity, UnitOfWork unitOfWork)
        {
            var consolidated = new ConsolidatedPurchaseDTO(rawSegments, activity);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        public static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            var consolidated = new ConsolidatedPurchaseDTO(backupSegments);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        private static void SaveFromConsolidated(ConsolidatedPurchaseDTO consolidated, UnitOfWork unitOfWork)
        {
            var value = int.Parse(consolidated.Value);
            var store = CompanyService.GetOrCreateByName(consolidated.StoreName, unitOfWork);

            var activity = ActivityService.Create(consolidated, unitOfWork);
            CreatePurchaseActivity(activity, store, consolidated.Product, value, consolidated.Description, unitOfWork);
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

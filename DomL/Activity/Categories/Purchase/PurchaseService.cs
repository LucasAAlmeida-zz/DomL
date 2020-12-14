using DomL.Business.DTOs;
using DomL.Business.Entities;
using DomL.Business.Utils;
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
            var consolidated = new PurchaseConsolidatedDTO(rawSegments, activity);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        public static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            var consolidated = new PurchaseConsolidatedDTO(backupSegments);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        private static void SaveFromConsolidated(PurchaseConsolidatedDTO consolidated, UnitOfWork unitOfWork)
        {
            var value = int.Parse(consolidated.Value);
            var activity = ActivityService.Create(consolidated, unitOfWork);
            CreatePurchaseActivity(activity, consolidated.Store, consolidated.Product, value, consolidated.Description, unitOfWork);
        }

        private static void CreatePurchaseActivity(Activity activity, string store, string product, int value, string description, UnitOfWork unitOfWork)
        {
            var purchaseActivity = new PurchaseActivity() {
                Activity = activity,
                Store = Util.GetStringOrNull(store),
                Product = Util.GetStringOrNull(product),
                Value = value,
                Description = Util.GetStringOrNull(description)
            };

            activity.PurchaseActivity = purchaseActivity;

            unitOfWork.PurchaseRepo.CreatePurchaseActivity(purchaseActivity);
        }
    }
}

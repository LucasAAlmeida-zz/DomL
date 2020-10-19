using DomL.Business.Entities;
using DomL.DataAccess;
using System;
using System.IO;
using System.Text.RegularExpressions;

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

        public static void RestoreFromFile(string fileDir)
        {
            using (var reader = new StreamReader(fileDir + "Purchase.txt")) {
                string line = "";
                while ((line = reader.ReadLine()) != null) {
                    if (string.IsNullOrWhiteSpace(line)) {
                        continue;
                    }

                    var segments = Regex.Split(line, "\t");

                    // Date; Store Name; Product; Value; (Description)
                    var date = segments[0];
                    var storeName = segments[1];
                    var product = segments[2];
                    var value = segments[3];
                    var description = segments[4] != "-" ? segments[4] : null;

                    var originalLine = "PURCHASE; " + storeName + "; " + product + "; " + value;
                    originalLine = (!string.IsNullOrWhiteSpace(description)) ? originalLine + "; " + description : originalLine;

                    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                        Company store = CompanyService.GetOrCreateByName(storeName, unitOfWork);
                        var statusSingle = unitOfWork.ActivityRepo.GetStatusById(ActivityStatus.SINGLE);
                        var category = unitOfWork.ActivityRepo.GetCategoryById(ActivityCategory.PURCHASE_ID);

                        var dateDT = DateTime.ParseExact(date, "dd/MM/yy", null);
                        var activity = ActivityService.Create(dateDT, 0, statusSingle, category, null, originalLine, unitOfWork);

                        CreatePurchaseActivity(activity, store, product, int.Parse(value), description, unitOfWork);

                        unitOfWork.Complete();
                    }
                }
            }
        }

        internal static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            throw new NotImplementedException();
        }
    }
}

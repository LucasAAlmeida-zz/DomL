using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class ConsolidatedPurchaseDTO : ActivityConsolidatedDTO
    {
        public string StoreName;
        public string Product;
        public string Value;
        public string Description;

        public ConsolidatedPurchaseDTO(Activity activity) : base(activity)
        {
            CategoryName = "PURCHASE";

            var purchaseActivity = activity.PurchaseActivity;
            var store = purchaseActivity.Store;

            StoreName = store.Name;
            Product = purchaseActivity.Product;
            Value = purchaseActivity.Value.ToString();
            Description = purchaseActivity.Description;
        }

        public ConsolidatedPurchaseDTO(string[] rawSegments, Activity activity) : base(activity)
        {
            CategoryName = "PURCHASE";

            StoreName = rawSegments[1];
            Product = rawSegments[2];
            Value = rawSegments[3];
            Description = (rawSegments.Length > 4) ? rawSegments[4] : null;
        }

        public ConsolidatedPurchaseDTO(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "PURCHASE";

            StoreName = backupSegments[4];
            Product = backupSegments[5];
            Value = backupSegments[6];
            Description = backupSegments[7];

            OriginalLine = GetInfoForOriginalLine()
                + GetPurchaseActivityInfo().Replace("\t", "; ");
        }

        public new string GetInfoForYearRecap()
        {
            return base.GetInfoForYearRecap()
                + "\t" + GetPurchaseActivityInfo();
        }

        public new string GetInfoForBackup()
        {
            return base.GetInfoForBackup()
                + "\t" + GetPurchaseActivityInfo();
        }

        public string GetPurchaseActivityInfo()
        {
            return StoreName + "\t" + Product + "\t" + Value + "\t" + Description;
        }
    }
}

using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class PurchaseConsolidatedDTO : ActivityConsolidatedDTO
    {
        public string Store;
        public string Product;
        public string Value;
        public string Description;

        public PurchaseConsolidatedDTO(Activity activity) : base(activity)
        {
            CategoryName = "PURCHASE";

            var purchaseActivity = activity.PurchaseActivity;

            Store = purchaseActivity.Store;
            Product = purchaseActivity.Product;
            Value = purchaseActivity.Value.ToString();
            Description = purchaseActivity.Description;
        }

        public PurchaseConsolidatedDTO(string[] rawSegments, Activity activity) : base(activity)
        {
            CategoryName = "PURCHASE";

            Store = rawSegments[1];
            Product = rawSegments[2];
            Value = rawSegments[3];
            Description = (rawSegments.Length > 4) ? rawSegments[4] : null;
        }

        public PurchaseConsolidatedDTO(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "PURCHASE";

            Store = backupSegments[4];
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
            return Store + "\t" + Product + "\t" + Value + "\t" + Description;
        }
    }
}

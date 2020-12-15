using DomL.Business.Entities;
using DomL.Business.Utils;

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
            var purchaseActivity = activity.PurchaseActivity;

            Store = Util.GetStringOrDash(purchaseActivity.Store);
            Product = Util.GetStringOrDash(purchaseActivity.Product);
            Value = Util.GetStringOrDash(purchaseActivity.Value.ToString());
            Description = Util.GetStringOrDash(purchaseActivity.Description);

            FillCommonInfo();
        }

        public PurchaseConsolidatedDTO(string[] rawSegments, Activity activity) : base(activity)
        {
            Store = Util.GetStringOrDash(rawSegments[1]);
            Product = Util.GetStringOrDash(rawSegments[2]);
            Value = Util.GetStringOrDash(rawSegments[3]);
            Description = Util.GetStringOrDash(rawSegments.Length > 4 ? rawSegments[4] : "-");

            FillCommonInfo();
        }

        public PurchaseConsolidatedDTO(string[] backupSegments) : base(backupSegments)
        {
            Store = backupSegments[4];
            Product = backupSegments[5];
            Value = backupSegments[6];
            Description = backupSegments[7];

            FillCommonInfo();
        }

        private void FillCommonInfo()
        {
            CategoryName = "PURCHASE";
            ConsolidatedLine = GetInfoForConsolidatedLine() + "; "
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

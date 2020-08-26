using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class ConsolidatedPurchaseDTO : ConsolidatedActivityDTO
    {
        public string StoreName;
        public string Product;
        public int Value;
        public string Description;

        public ConsolidatedPurchaseDTO(Activity activity) : base(activity)
        {
            var purchaseActivity = activity.PurchaseActivity;
            var store = purchaseActivity.Store;

            StoreName = store.Name;
            Product = purchaseActivity.Product;
            Value = purchaseActivity.Value;
            Description = purchaseActivity.Description;
        }

        public string GetInfoForYearRecap()
        {
            // Date Started; Date Finished;
            // Store Name; Product; Value; Description
            return DatesStartAndFinish
                + "\t" + StoreName + "\t" + Product + "\t" + Value + "\t" + Description;
        }
    }
}

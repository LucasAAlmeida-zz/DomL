using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class GiftConsolidatedDTO : ActivityConsolidatedDTO
    {
        public string Gift;
        public string IsToOrFrom;
        public string Who;
        public string Description;

        public GiftConsolidatedDTO(Activity activity) : base(activity)
        {
            var giftActivity = activity.GiftActivity;

            Gift = giftActivity.Gift;
            IsToOrFrom = giftActivity.IsFrom ? "From" : "To";
            Who = giftActivity.Who;
            Description = giftActivity.Description;
        }
        public GiftConsolidatedDTO(string[] rawSegments, Activity activity) : base(activity)
        {
            Gift = rawSegments[1];
            IsToOrFrom = rawSegments[2];
            Who = rawSegments[3];
            Description = (rawSegments.Length > 4) ? rawSegments[4] : null;
        }

        public GiftConsolidatedDTO(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "GIFT";

            Gift = backupSegments[4];
            IsToOrFrom = backupSegments[5];
            Who = backupSegments[6];
            Description = backupSegments[7];

            OriginalLine = GetInfoForOriginalLine() + "; "
                + GetGiftActivityInfo().Replace("\t", "; ");
        }

        public new string GetInfoForYearRecap()
        {
            return base.GetInfoForYearRecap()
                + "\t" + GetGiftActivityInfo();
        }

        public new string GetInfoForBackup()
        {
            return base.GetInfoForBackup()
                + "\t" + GetGiftActivityInfo();
        }

        public string GetGiftActivityInfo()
        {
            return Gift + "\t" + IsToOrFrom + "\t" + Who + "\t" + Description;
        }
    }
}

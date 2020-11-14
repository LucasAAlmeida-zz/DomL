using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class GiftConsolidatedDTO : ActivityConsolidatedDTO
    {
        public string Who;
        public string IsToOrFrom;
        public string Gift;
        public string Description;

        public GiftConsolidatedDTO(Activity activity) : base(activity)
        {
            var giftActivity = activity.GiftActivity;

            Who = giftActivity.Who;
            IsToOrFrom = giftActivity.IsFrom ? "From" : "To";
            Gift = giftActivity.Gift;

            Description = giftActivity.Description;
        }
        public GiftConsolidatedDTO(string[] rawSegments, Activity activity) : this(activity)
        {
            Gift = rawSegments[1];
            IsToOrFrom = rawSegments[2];
            Who = rawSegments[3];
            Description = (rawSegments.Length > 4) ? rawSegments[4] : null;
        }

        public GiftConsolidatedDTO(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "GIFT";

            Who = backupSegments[4];
            IsToOrFrom = backupSegments[5];
            Gift = backupSegments[6];

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

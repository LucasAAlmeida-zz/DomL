using DomL.Business.Entities;
using DomL.Business.Utils;

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

            Gift = Util.GetStringOrDash(giftActivity.Gift);
            IsToOrFrom = giftActivity.IsFrom ? "From" : "To";
            Who = Util.GetStringOrDash(giftActivity.Who);
            Description = Util.GetStringOrDash(giftActivity.Description);

            FillCommonInfo();
        }
        public GiftConsolidatedDTO(string[] rawSegments, Activity activity) : base(activity)
        {
            Gift = Util.GetStringOrDash(rawSegments[1]);
            IsToOrFrom = Util.GetStringOrDash(rawSegments[2]);
            Who = Util.GetStringOrDash(rawSegments[3]);
            Description = Util.GetStringOrDash(rawSegments.Length > 4 ? rawSegments[4] : "-");

            FillCommonInfo();
        }

        public GiftConsolidatedDTO(string[] backupSegments) : base(backupSegments)
        {
            Gift = backupSegments[4];
            IsToOrFrom = backupSegments[5];
            Who = backupSegments[6];
            Description = backupSegments[7];

            FillCommonInfo();
        }

        private void FillCommonInfo()
        {
            CategoryName = "GIFT";
            ConsolidatedLine = GetInfoForConsolidatedLine() + "; "
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

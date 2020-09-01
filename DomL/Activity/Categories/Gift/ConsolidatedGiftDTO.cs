using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class ConsolidatedGiftDTO : ConsolidatedActivityDTO
    {
        public string Who;
        public string IsToOrFrom;
        public string Gift;
        public string Description;

        public ConsolidatedGiftDTO(Activity activity) : base(activity)
        {
            var giftActivity = activity.GiftActivity;

            Who = giftActivity.Who;
            IsToOrFrom = giftActivity.IsFrom ? "From" : "To";
            Gift = giftActivity.Gift;

            Description = giftActivity.Description;
        }

        public string GetInfoForYearRecap()
        {
            // Date Started; Date Finished;
            // Gift; Is To or From; Person; Description
            return DatesStartAndFinish
                + "\t" + Gift + "\t" + IsToOrFrom + "\t" + Who + "\t" + Description;
        }
    }
}

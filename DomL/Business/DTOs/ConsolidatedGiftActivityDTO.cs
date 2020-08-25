using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class ConsolidatedGiftActivityDTO : ConsolidatedActivityDTO
    {
        public string PersonName;
        public string IsToOrFrom;
        public string Gift;
        public string Description;

        public ConsolidatedGiftActivityDTO(Activity activity) : base(activity)
        {
            var giftActivity = activity.GiftActivity;

            PersonName = giftActivity.Person.Name;
            IsToOrFrom = giftActivity.IsFrom ? "From" : "To";
            Gift = giftActivity.Gift;

            Description = giftActivity.Description;
        }

        public string GetInfoForYearRecap()
        {
            // Date Started; Date Finished;
            // Gift; Is To or From; Person; Description
            return DatesStartAndFinish
                + "\t" + Gift + "\t" + IsToOrFrom + "\t" + PersonName + "\t" + Description;
        }
    }
}

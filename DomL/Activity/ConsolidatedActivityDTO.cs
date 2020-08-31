using DomL.Business.Entities;
using DomL.Business.Services;
using DomL.Business.Utils;

namespace DomL.Business.DTOs
{
    public class ConsolidatedActivityDTO
    {
        public string OriginalLine;
        public string Date;

        public string DatesStartAndFinish;

        public ConsolidatedActivityDTO(Activity activity)
        {
            OriginalLine = activity.OriginalLine;
            Date = Util.GetFormatedDate(activity.Date);

            var pairedDate = (activity.PairedActivity != null) ? Util.GetFormatedDate(activity.PairedActivity.Date) : "????/??/??";

            if (activity.StatusId == ActivityStatus.SINGLE) {
                DatesStartAndFinish = "----------\t" + Date;
            } else if (activity.StatusId == ActivityStatus.START) {
                DatesStartAndFinish = Date + "\t" + pairedDate;
            } else if (activity.StatusId == ActivityStatus.FINISH) {
                DatesStartAndFinish = pairedDate + "\t" + Date;
            }
        }

        public string GetInfoForMonthRecap()
        {
            // Date; OriginalLine
            return Date + "\t" + OriginalLine;
        }
    }
}
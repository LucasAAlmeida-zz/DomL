using DomL.Business.Entities;
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

            if (activity.StatusId == ActivityStatus.SINGLE) {
                DatesStartAndFinish = "----------\t" + Date;
            } else if (activity.StatusId == ActivityStatus.START) {
                DatesStartAndFinish = Date + "\t" + "????/??/??";
            } else if (activity.StatusId == ActivityStatus.FINISH) {
                DatesStartAndFinish = "????/??/??" + "\t" + Date;
            }
        }

        public string GetInfoForMonthRecap()
        {
            // Date; OriginalLine
            return Date + "\t" + OriginalLine;
        }
    }
}
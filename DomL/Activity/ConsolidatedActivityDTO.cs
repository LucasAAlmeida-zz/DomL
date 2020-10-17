using DomL.Business.Entities;
using DomL.Business.Utils;

namespace DomL.Business.DTOs
{
    public class ConsolidatedActivityDTO
    {
        public string OriginalLine;
        public string Date;

        public string DayOrder;
        public string StatusName;
        public string ActivityBlockName;

        public string DatesStartAndFinish;

        public ConsolidatedActivityDTO(Activity activity)
        {
            OriginalLine = activity.OriginalLine;
            Date = Util.GetFormatedDate(activity.Date);

            DayOrder = activity.DayOrder.ToString();
            StatusName = "-";

            var pairedDate = (activity.PairedActivity != null) ? Util.GetFormatedDate(activity.PairedActivity.Date) : "????/??/??";
            switch (activity.StatusId) {
                case ActivityStatus.SINGLE:
                    DatesStartAndFinish = "----------\t" + Date;
                    break;
                case ActivityStatus.START:
                    StatusName = "Start";
                    DatesStartAndFinish = Date + "\t" + pairedDate;
                    break;
                case ActivityStatus.FINISH:
                    StatusName = "Finish";
                    DatesStartAndFinish = pairedDate + "\t" + Date;
                    break;
            }

            ActivityBlockName = (activity.ActivityBlock != null) ? activity.ActivityBlock.Name : "-";
        }

        public string GetInfoForMonthRecap()
        {
            // Date; OriginalLine
            return Date + "\t" + OriginalLine;
        }

        protected string GetInfoForBackup()
        {
            return Date + "\t" + DayOrder + "\t" + ActivityBlockName + "\t" + StatusName;
        }
    }
}
using DomL.Business.Entities;
using DomL.Business.Utils;

namespace DomL.Business.DTOs
{
    public class ActivityConsolidatedDTO
    {
        public string Date;

        public string DayOrder;
        public string StatusName;
        public string CategoryName;
        public string BlockName;

        public string OriginalLine;
        public string DatesStartAndFinish;

        public ActivityConsolidatedDTO(Activity activity)
        {
            OriginalLine = activity.OriginalLine;
            Date = Util.GetFormatedDate(activity.Date);

            DayOrder = activity.DayOrder.ToString();

            var pairedDate = (activity.PairedActivity != null) ? Util.GetFormatedDate(activity.PairedActivity.Date) : "????/??/??";
            switch (activity.StatusId) {
                case ActivityStatus.SINGLE:
                    StatusName = "Single";
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

            BlockName = (activity.ActivityBlock != null) ? activity.ActivityBlock.Name : "-";
        }

        public ActivityConsolidatedDTO(string[] segments)
        {
            Date = segments[0];
            DayOrder = segments[1];
            BlockName = segments[2];
            StatusName = segments[3];
        }

        protected string GetInfoForOriginalLine()
        {
            string statusName = (StatusName != "-" && StatusName != "Single") ? " " + StatusName : "";
            string blockName = BlockName != "-" ? " " + BlockName : "";

            return CategoryName + statusName + blockName;
        }

        public string GetInfoForMonthRecap()
        {
            return Date + "\t" + OriginalLine;
        }

        protected string GetInfoForYearRecap()
        {
            return DatesStartAndFinish;
        }

        protected string GetInfoForBackup()
        {
            return Date + "\t" + DayOrder + "\t" + BlockName + "\t" + StatusName;
        }
    }
}
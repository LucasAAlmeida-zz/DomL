using DomL.Business.Entities;
using DomL.Business.Utils;

namespace DomL.Business.DTOs
{
    public abstract class ConsolidatedActivityDTO
    {
        public string Category;
        public string Date;
        public string PairedDate;
        public string DateStart;
        public string DateFinish;
        public string DayOrder;
        public string Status;
        public string ActivityBlockName;

        public ConsolidatedActivityDTO(Activity activity)
        {
            Category = activity.Category.Name;
            Date = Util.GetFormatedDate(activity.Date);
            DayOrder = activity.DayOrder.ToString("000");
            Status = activity.Status.Name;
            ActivityBlockName = (activity.ActivityBlock != null) ? activity.ActivityBlock.Name : "-";
            PairedDate = (activity.PairedActivity != null) ? Util.GetFormatedDate(activity.PairedActivity.Date) : "????/??/??";

            DateStart = Date;
            DateFinish = Date;
            if (activity.Status.Id == ActivityStatus.START) {
                DateFinish = PairedDate;
            } else if (activity.Status.Id == ActivityStatus.FINISH) {
                DateStart = PairedDate;
            }
        }

        protected string GetActivityInfoForMonthRecap()
        {
            var status = Status != "SINGLE" ? Status : "------";
            return Date + "\t" + Category + "\t" + status;
        }

        protected string GetActivityInfoForYearRecap()
        {
            return (Status == "SINGLE") ? "----------\t" + Date : DateStart + "\t" + DateFinish;
        }

        protected string GetActivityInfoForBackup()
        {
            return Category + "\t" + Date + "\t" + DayOrder
                + "\t" + Status + "\t" + ActivityBlockName;
        }

        public string GetInfoForYearRecap()
        {
            if (this.Status == "START" && !this.PairedDate.StartsWith("??")) {
                return "";
            }
            return GetInfoForYearRecapChild();
        }

        public abstract string GetInfoForMonthRecap();
        protected abstract string GetInfoForYearRecapChild();
        public abstract string GetInfoForBackup();
    }
}
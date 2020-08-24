using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class ConsolidatedAutoActivityDTO : ConsolidatedActivityDTO
    {
        public string AutoName;
        public string Description;

        public ConsolidatedAutoActivityDTO(Activity activity) : base(activity)
        {
            var autoActivity = activity.AutoActivity;
            var auto = autoActivity.Auto;

            AutoName = auto.Name;
            Description = autoActivity.Description;
        }

        public override string GetInfoForMonthRecap()
        {
            // Date; Category; Auto Name; Description
            return this.GetActivityInfoForMonthRecap() + "\t" + this.GetAutoInfo();
        }

        protected override string GetInfoForYearRecapChild()
        {
            // Date; Auto Name; Description
            return this.GetActivityInfoForYearRecap() + "\t" + this.GetAutoInfo();
        }

        public override string GetInfoForBackup()
        {
            // Category; Date; Day Order; Status; Activity Block Name;
            // Auto Name; Description
            return this.GetActivityInfoForBackup() + "\t" + this.GetAutoInfo();
        }

        private string GetAutoInfo()
        {
            return AutoName + "\t" + Description;
        }
    }
}

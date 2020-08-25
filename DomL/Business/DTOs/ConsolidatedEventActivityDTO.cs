using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class ConsolidatedEventActivityDTO : ConsolidatedActivityDTO
    {
        public string Description;
        public bool IsImportant;

        public ConsolidatedEventActivityDTO(Activity activity) : base(activity)
        {
            var eventActivity = activity.EventActivity;

            Description = eventActivity.Description;
            IsImportant = eventActivity.IsImportant;
        }

        public override string GetInfoForMonthRecap()
        {
            if (ActivityBlockName == "-" && !IsImportant) {
                return "";
            }

            // Date; Category; Description
            return this.GetActivityInfoForMonthRecap() + "\t" + this.GetEventInfo();
        }

        protected override string GetInfoForYearRecapChild()
        {
            if (!IsImportant) {
                return "";
            }

            // Date; Description
            return this.GetActivityInfoForYearRecap() + "\t" + this.GetEventInfo();
        }

        public override string GetInfoForBackup()
        {
            // Category; Date; Day Order; Status; Activity Block Name;
            // Description
            // IsImportant
            var isImportant = IsImportant ? "Important" : "Not Important";
            return this.GetActivityInfoForBackup() + "\t" + this.GetEventInfo() + "\t" + isImportant;
        }

        private string GetEventInfo()
        {
            return Description;
        }
    }
}

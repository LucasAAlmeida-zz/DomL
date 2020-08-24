using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class ConsolidatedDoomActivityDTO : ConsolidatedActivityDTO
    {
        public string Description;

        public ConsolidatedDoomActivityDTO(Activity activity) : base(activity)
        {
            var doomActivity = activity.DoomActivity;

            Description = doomActivity.Description;
        }

        public override string GetInfoForMonthRecap()
        {
            // Date; Category; Description
            return this.GetActivityInfoForMonthRecap() + "\t" + this.GetDoomInfo();
        }

        protected override string GetInfoForYearRecapChild()
        {
            // Date; Description
            return this.GetActivityInfoForYearRecap() + "\t" + this.GetDoomInfo();
        }

        public override string GetInfoForBackup()
        {
            // Category; Date; Day Order; Status; Activity Block Name;
            // Description
            return this.GetActivityInfoForBackup() + "\t" + this.GetDoomInfo();
        }

        private string GetDoomInfo()
        {
            return Description;
        }
    }
}

using DomL.Business.Entities;
using DomL.Business.Utils;

namespace DomL.Business.DTOs
{
    public class EventConsolidatedDTO : ActivityConsolidatedDTO
    {
        public string Description;
        public bool IsImportant;

        public EventConsolidatedDTO(Activity activity) : base(activity)
        {
            var eventActivity = activity.EventActivity;

            Description = Util.GetStringOrDash(eventActivity.Description);
            IsImportant = eventActivity.IsImportant;

            FillCommonInfo();
        }

        public EventConsolidatedDTO(string[] rawSegments, Activity activity) : base(activity)
        {
            Description = Util.GetStringOrDash(rawSegments[0]);
            IsImportant = false;

            FillCommonInfo();

            if (Description.StartsWith("*")) {
                IsImportant = true;
                Description = Description.Substring(1);
            }
        }

        public EventConsolidatedDTO(string[] backupSegments) : base(backupSegments)
        {
            Description = backupSegments[4];
            IsImportant = false;

            FillCommonInfo();

            if (Description.StartsWith("*")) {
                IsImportant = true;
                Description = Description.Substring(1);
            }
        }

        private void FillCommonInfo()
        {
            CategoryName = "EVENT";
            ConsolidatedLine = GetInfoForConsolidatedLine() + "; "
                + GetEventActivityInfo().Replace("\t", "; ");
        }

        public new string GetInfoForYearRecap()
        {
            if (!IsImportant) {
                return "";
            }

            return base.GetInfoForYearRecap()
                + "\t" + GetEventActivityInfo();
        }

        public new string GetInfoForBackup()
        {
            var isImportantMarker = IsImportant ? "*" : "";
            return base.GetInfoForBackup()
                + "\t" + isImportantMarker + GetEventActivityInfo();
        }

        public string GetEventActivityInfo()
        {
            return Description;
        }
    }
}

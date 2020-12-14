using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class EventConsolidatedDTO : ActivityConsolidatedDTO
    {
        public string Description;
        public bool IsImportant;

        public EventConsolidatedDTO(Activity activity) : base(activity)
        {
            var eventActivity = activity.EventActivity;

            Description = eventActivity.Description;
            IsImportant = eventActivity.IsImportant;
        }

        public EventConsolidatedDTO(string[] rawSegments, Activity activity) : base(activity)
        {
            CategoryName = "EVENT";

            Description = rawSegments[0];
            IsImportant = false;

            if (Description.StartsWith("*")) {
                IsImportant = true;
                Description = Description.Substring(1);
            }
        }

        public EventConsolidatedDTO(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "EVENT";

            Description = backupSegments[4];
            IsImportant = false;

            OriginalLine = GetInfoForOriginalLine() + "; "
                + GetEventActivityInfo().Replace("\t", "; ");

            if (Description.StartsWith("*")) {
                IsImportant = true;
                Description = Description.Substring(1);
            }
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

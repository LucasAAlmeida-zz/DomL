using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class ConsolidatedEventDTO : ActivityConsolidatedDTO
    {
        public string Description;
        public bool IsImportant;

        public ConsolidatedEventDTO(Activity activity) : base(activity)
        {
            var eventActivity = activity.EventActivity;

            Description = eventActivity.Description;
            IsImportant = eventActivity.IsImportant;
        }

        public ConsolidatedEventDTO(string[] rawSegments, Activity activity) : this(activity)
        {
            CategoryName = "EVENT";

            Description = rawSegments[0];
            IsImportant = false;

            if (Description.StartsWith("*")) {
                IsImportant = true;
                Description = Description.Substring(1);
            }
        }

        public ConsolidatedEventDTO(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "EVENT";

            Description = backupSegments[4];
            IsImportant = false;

            if (Description.StartsWith("*")) {
                IsImportant = true;
                Description = Description.Substring(1);
            }

            OriginalLine = GetInfoForOriginalLine() + "; "
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

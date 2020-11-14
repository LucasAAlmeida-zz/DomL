using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class DoomConsolidatedDTO : ActivityConsolidatedDTO
    {
        public string Description;

        public DoomConsolidatedDTO(Activity activity) : base(activity)
        {
            CategoryName = "DOOM";

            var doomActivity = activity.DoomActivity;

            Description = doomActivity.Description;
        }

        public DoomConsolidatedDTO(string[] rawSegments, Activity activity) : this(activity)
        {
            CategoryName = "DOOM";

            Description = rawSegments[1];
        }

        public DoomConsolidatedDTO(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "DOOM";

            Description = backupSegments[4];

            OriginalLine = GetInfoForOriginalLine() + "; "
                + GetDoomActivityInfo().Replace("\t", "; ");
        }

        public new string GetInfoForYearRecap()
        {
            return base.GetInfoForYearRecap()
                + "\t" + GetDoomActivityInfo();
        }

        public new string GetInfoForBackup()
        {
            return base.GetInfoForBackup()
                + "\t" + GetDoomActivityInfo();
        }

        public string GetDoomActivityInfo()
        {
            return Description;
        }
    }
}

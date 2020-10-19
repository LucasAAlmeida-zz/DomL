using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class ConsolidatedDoomDTO : ConsolidatedActivityDTO
    {
        public string Description;

        public ConsolidatedDoomDTO(Activity activity) : base(activity)
        {
            var doomActivity = activity.DoomActivity;

            Description = doomActivity.Description;
        }

        public ConsolidatedDoomDTO(string[] backupSegments) : base(backupSegments)
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

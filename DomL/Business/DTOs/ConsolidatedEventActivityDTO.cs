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

        public string GetInfoForYearRecap()
        {
            if (!IsImportant) {
                return "";
            }
            // Date Started; Date Finished;
            // Description
            return DatesStartAndFinish + "\t" + Description;
        }
    }
}

using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class ConsolidatedWorkDTO : ConsolidatedActivityDTO
    {
        public string WorkName;
        public string Description;

        public ConsolidatedWorkDTO(Activity activity) : base(activity)
        {
            var workActivity = activity.WorkActivity;
            var work = workActivity.Work;

            WorkName = work.Name;
            Description = workActivity.Description;
        }

        public string GetInfoForYearRecap()
        {
            // Date Started; Date Finished;
            // Work Name; Description
            return DatesStartAndFinish
                + "\t" + WorkName + "\t" + Description;
        }
    }
}

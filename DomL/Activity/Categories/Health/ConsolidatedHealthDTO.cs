using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class ConsolidatedHealthDTO : ConsolidatedActivityDTO
    {
        public string MedicalSpecialtyName;
        public string Description;

        public ConsolidatedHealthDTO(Activity activity) : base(activity)
        {
            var healthActivity = activity.HealthActivity;
            var specialty = healthActivity.Specialty;

            MedicalSpecialtyName = (specialty != null) ? specialty.Name : "-";
            Description = healthActivity.Description;
        }

        public string GetInfoForYearRecap()
        {
            // Date Started; Date Finished;
            // Medical Specialty Name; Description
            return DatesStartAndFinish
                + "\t" + GetHealthActivityInfo();
        }

        public new string GetInfoForBackup()
        {
            return base.GetInfoForBackup()
                + "\t" + GetHealthActivityInfo();
        }

        public string GetHealthActivityInfo()
        {
            return MedicalSpecialtyName + "\t" + Description;
        }
    }
}

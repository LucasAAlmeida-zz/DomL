
using DomL.Business.Entities;
using System.Linq;

namespace DomL.DataAccess
{
    public class HealthRepository : DomLRepository<HealthActivity>
    {
        public HealthRepository(DomLContext context) : base(context) { }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }

        public MedicalSpecialty GetMedicalSpecialtyByName(string medicalSpecialtyName)
        {
            return DomLContext.MedicalSpecialty.SingleOrDefault(a => a.Name == medicalSpecialtyName);
        }

        public void CreateMedicalSpecialty(MedicalSpecialty medicalSpecialty)
        {
            DomLContext.MedicalSpecialty.Add(medicalSpecialty);
        }

        public void CreateHealthActivity(HealthActivity healthActivity)
        {
            DomLContext.HealthActivity.Add(healthActivity);
        }
    }
}

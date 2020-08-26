using DomL.Business.DTOs;
using DomL.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DomL.Business.Services
{
    public class HealthService
    {
        public static void SaveFromRawSegments(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            // HEALTH; (Medical Specialty Name); Description
            string medicalSpecialtyName = null;
            string description;
            if (segments.Length == 2) {
                description = segments[1];
            } else {
                medicalSpecialtyName = segments[1];
                description = segments[2];
            }

            MedicalSpecialty specialty = GetOrCreateMedicalSpecialtyByName(medicalSpecialtyName, unitOfWork);

            CreateHealthActivity(activity, specialty, description, unitOfWork);
        }

        private static MedicalSpecialty GetOrCreateMedicalSpecialtyByName(string medicalSpecialtyName, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(medicalSpecialtyName)) {
                return null;
            }

            var specialty = unitOfWork.HealthRepo.GetMedicalSpecialtyByName(medicalSpecialtyName);

            if (specialty == null) {
                specialty = new MedicalSpecialty() {
                    Name = medicalSpecialtyName
                };
                unitOfWork.HealthRepo.CreateMedicalSpecialty(specialty);
            }

            return specialty;
        }

        private static void CreateHealthActivity(Activity activity, MedicalSpecialty specialty, string description, UnitOfWork unitOfWork)
        {
            var healthActivity = new HealthActivity() {
                Activity = activity,
                Specialty = specialty,
                Description = description
            };

            activity.HealthActivity = healthActivity;
            activity.PairUpActivity(unitOfWork);

            unitOfWork.HealthRepo.CreateHealthActivity(healthActivity);
        }

        public static IEnumerable<Activity> GetStartingActivities(IQueryable<Activity> previousStartingActivities, Activity activity)
        {
            var healthActivity = activity.HealthActivity;
            return previousStartingActivities.Where(u =>
                u.CategoryId == ActivityCategory.HEALTH
                && u.HealthActivity.Description == healthActivity.Description
            );
        }
    }
}

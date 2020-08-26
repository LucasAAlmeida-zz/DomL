using DomL.Business.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DomL.Business.Services
{
    public class LocationService
    {
        public static Location GetOrCreateByName(string locationName, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(locationName)) {
                return null;
            }

            var location = GetByName(locationName, unitOfWork);

            if (location == null) {
                location = new Location() {
                    Name = locationName
                };
                unitOfWork.LocationRepo.Add(location);
            }

            return location;
        }

        public static Location GetByName(string locationName, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(locationName)) {
                return null;
            }
            return unitOfWork.LocationRepo.GetByName(locationName);
        }
    }
}
using DomL.Business.Entities;

namespace DomL.Business.Services
{
    public class TransportService
    {
        public static Transport CreateOrGetByName(string transportName, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(transportName)) {
                return null;
            }

            var transport = GetByName(transportName, unitOfWork);

            if (transport == null) {
                transport = new Transport() {
                    Name = transportName
                };
                unitOfWork.TransportRepo.Add(transport);
            }

            return transport;
        }

        public static Transport GetByName(string transportName, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(transportName)) {
                return null;
            }
            return unitOfWork.TransportRepo.GetByName(transportName);
        }
    }
}
using DomL.Business.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DomL.Business.Services
{
    public class CompanyService
    {
        public static Company GetOrCreateByName(string companyName, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(companyName)) {
                return null;
            }

            var company = GetByName(companyName, unitOfWork);

            if (company == null) {
                company = new Company() {
                    Name = companyName
                };
                unitOfWork.CompanyRepo.Add(company);
            }

            return company;
        }

        public static List<Company> GetAll(UnitOfWork unitOfWork)
        {
            return unitOfWork.CompanyRepo.GetAll().ToList();
        }

        public static Company GetByName(string companyName, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(companyName)) {
                return null;
            }
            return unitOfWork.CompanyRepo.GetByName(companyName);
        }
    }
}
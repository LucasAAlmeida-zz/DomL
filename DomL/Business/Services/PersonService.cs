using DomL.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DomL.Business.Services
{
    public class PersonService
    {
        public static Person GetOrCreateByName(string personName, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(personName)) {
                return null;
            }

            var person = GetByName(personName, unitOfWork);

            if (person == null) {
                person = CreatePerson(unitOfWork, personName);
            }

            return person;
        }

        public static Person GetOrCreateByNameAndOrigin(string personName, string origin, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(personName) || string.IsNullOrWhiteSpace(origin)) {
                return null;
            }

            var person = GetByNameAndOrigin(personName, origin, unitOfWork);

            if (person == null) {
                person = CreatePerson(unitOfWork, personName, origin);
            }

            return person;
        }

        private static Person CreatePerson(UnitOfWork unitOfWork, string personName, string origin = null)
        {
            Person person = new Person() {
                Name = personName,
                Origin = origin
            };
            unitOfWork.PersonRepo.Add(person);
            return person;
        }

        public static List<Person> GetAll(UnitOfWork unitOfWork)
        {
            return unitOfWork.PersonRepo.GetAll().ToList();
        }

        public static Person GetByName(string personName, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(personName)) {
                return null;
            }
            return unitOfWork.PersonRepo.GetByName(personName);
        }

        public static Person GetByNameAndOrigin(string personName, string origin, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(personName) || string.IsNullOrWhiteSpace(origin)) {
                return null;
            }
            return unitOfWork.PersonRepo.GetByNameAndOrigin(personName, origin);
        }
    }
}
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
                person = CreatePerson(personName, unitOfWork);
            }

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

        public static Person CreatePerson(string personName, UnitOfWork unitOfWork)
        {
            Person person = new Person() {
                Name = personName,
            };
            unitOfWork.PersonRepo.Add(person);
            return person;
        }
    }
}
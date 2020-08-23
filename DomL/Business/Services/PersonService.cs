using DomL.Business.Entities;
using DomL.Business.Utils;

namespace DomL.Business.Services
{
    public class PersonService
    {
        public static Person GetOrCreateByName(string personName, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(personName)) {
                return null;
            }

            var cleanPersonName = Util.CleanString(personName);
            Person person = unitOfWork.PersonRepo.SingleOrDefault(u =>
                u.Name.Replace(":", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").Replace("'", "").Replace(",", "").ToLower().Replace("the", "")
                == cleanPersonName
            );

            if (person == null) {
                person = new Person() {
                    Name = personName
                };
                unitOfWork.PersonRepo.Add(person);
            }

            return person;
        }
    }
}
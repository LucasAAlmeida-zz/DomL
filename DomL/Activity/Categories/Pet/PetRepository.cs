using DomL.Business.Entities;
using System;
using System.Linq;

namespace DomL.DataAccess
{
    public class PetRepository : DomLRepository<PetActivity>
    {
        public PetRepository(DomLContext context) : base(context) { }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }

        public Pet GetPetByName(string name)
        {
            return DomLContext.Pet.SingleOrDefault(a => a.Name == name);
        }

        public void CreatePetActivity(PetActivity petActivity)
        {
            DomLContext.PetActivity.Add(petActivity);
        }

        public void CreatePet(Pet pet)
        {
            DomLContext.Pet.Add(pet);
        }
    }
}

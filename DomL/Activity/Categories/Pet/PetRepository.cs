using DomL.Business.Entities;

namespace DomL.DataAccess
{
    public class PetRepository : DomLRepository<PetActivity>
    {
        public PetRepository(DomLContext context) : base(context) { }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }

        public void CreatePetActivity(PetActivity petActivity)
        {
            DomLContext.PetActivity.Add(petActivity);
        }
    }
}

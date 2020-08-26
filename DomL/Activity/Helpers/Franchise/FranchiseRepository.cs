using DomL.Business.Entities;
using DomL.Business.Utils;
using System.Linq;

namespace DomL.DataAccess
{
    public class FranchiseRepository : BaseRepository<Franchise>
    {
        public FranchiseRepository(DomLContext context) : base(context) { }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }

        public Franchise GetByName(string franchiseName)
        {
            var cleanFranchiseName = Util.CleanString(franchiseName);
            return DomLContext.Franchise.SingleOrDefault(u =>
                u.Name.Replace(":", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").Replace("'", "").Replace(",", "").ToLower().Replace("the", "")
                == cleanFranchiseName
            );
        }
    }

}

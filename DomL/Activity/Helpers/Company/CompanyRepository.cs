using DomL.Business.Entities;
using DomL.Business.Utils;
using System.Linq;

namespace DomL.DataAccess
{
    public class CompanyRepository : DomLRepository<Company>
    {
        public CompanyRepository(DomLContext context) : base(context) { }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }

        public Company GetByName(string companyName)
        {
            var cleanCompanyName = Util.CleanString(companyName);
            return DomLContext.Company.SingleOrDefault(u =>
                u.Name.Replace(":", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").Replace("'", "").Replace(",", "").ToLower().Replace("the", "")
                == cleanCompanyName
            );
        }
    }

}

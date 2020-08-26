using DomL.Business.Entities;
using DomL.Business.Utils;
using System.Linq;

namespace DomL.DataAccess
{
    public class TransportRepository : BaseRepository<Transport>
    {
        public TransportRepository(DomLContext context) : base(context) { }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }

        public Transport GetByName(string transportName)
        {
            var cleanTransportName = Util.CleanString(transportName);
            return DomLContext.Transport.SingleOrDefault(u =>
                u.Name.Replace(":", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").Replace("'", "").Replace(",", "").ToLower().Replace("the", "")
                == cleanTransportName
            );
        }
    }
}

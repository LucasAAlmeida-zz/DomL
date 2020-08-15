using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomL.Business.Utils
{
    public class ParseException : Exception
    {
        public ParseException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

using System;

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

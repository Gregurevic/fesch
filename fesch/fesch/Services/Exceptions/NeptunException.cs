using System;

namespace fesch.Services.Exceptions
{
    public class NeptunException : Exception
    {
        public NeptunException()
        {
        }

        public NeptunException(string message)
            : base(message)
        {
        }

        public NeptunException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

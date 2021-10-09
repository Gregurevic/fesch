using System;

namespace fesch.Services.Exceptions
{
    class StructureSchedulerException : Exception
    {
        public StructureSchedulerException()
        {
        }

        public StructureSchedulerException(string message)
            : base(message)
        {
        }

        public StructureSchedulerException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

using System;

namespace fesch.Services.Exceptions
{
    class AttendantSchedulerException : Exception
    {
        public AttendantSchedulerException()
        {
        }

        public AttendantSchedulerException(string message)
            : base(message)
        {
        }

        public AttendantSchedulerException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

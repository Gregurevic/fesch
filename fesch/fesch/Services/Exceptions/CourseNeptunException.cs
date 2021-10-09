using System;

namespace fesch.Services.Exceptions
{
    class CourseNeptunException : Exception
    {
        public CourseNeptunException()
        {
        }

        public CourseNeptunException(string message)
            : base(message)
        {
        }

        public CourseNeptunException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

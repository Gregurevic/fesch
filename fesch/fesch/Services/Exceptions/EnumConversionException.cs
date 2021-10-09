using System;
namespace fesch.Services.Exceptions
{
    class EnumConversionException : Exception
    {
        public EnumConversionException()
        {
        }

        public EnumConversionException(string message)
            : base(message)
        {
        }

        public EnumConversionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

using System;

namespace ALPHASim.Exceptions
{
    public class InvalidDataTypeException : Exception
    {
        public InvalidDataTypeException() { }

        public InvalidDataTypeException(string message)
            : base (message) { }

        public InvalidDataTypeException(string message, Exception inner)
            : base(message, inner) { }
    }
}

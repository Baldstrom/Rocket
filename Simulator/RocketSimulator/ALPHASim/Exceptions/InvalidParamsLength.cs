using System;

namespace ALPHASim.Exceptions
{
    public class InvalidParamsLength : Exception
    {
        public InvalidParamsLength() { }

        public InvalidParamsLength(string message)
            : base(message) { }

        public InvalidParamsLength(string message, Exception inner)
            : base(message, inner) { }
    }
}

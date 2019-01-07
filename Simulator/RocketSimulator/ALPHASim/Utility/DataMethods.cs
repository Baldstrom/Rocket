using ALPHASim.Exceptions;
using System;

namespace ALPHASim.Utility
{
    public static class DataMethods
    {
        /// <summary> Returns whether or not <c>type</c> is a numeric. </summary>
        /// <param name="type"> The type under test. </param>
        /// <returns> Whether or not <c>type</c> is numberic. </returns>
        public static bool TypeIsNumeric(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary> Ensures that <c>type</c> is numeric by throwing an exception otherwise. </summary>
        /// <param name="type"> The type to test if numeric. </param>
        /// <param name="message"> The message to add to the exception if it is thrown. </param>
        /// <exception cref="InvalidDataTypeException">If <c>type</c> is non-numeric. </exception>
        public static void EnsureIsNumeric(Type type, string message = null)
        {
            if (Constants.CustomNumerics.Contains(type)) { return; }
            if (!TypeIsNumeric(type)) { throw new InvalidDataTypeException(message ?? ""); }
        }

    }
}

using System;
using JetBrains.Annotations;

namespace Cql.Core.Common.Exceptions
{
    public class CqlException : Exception
    {
        [StringFormatMethod("format")]
        public CqlException(string format, params object[] args) : this(string.Format(format, args))
        {
        }

        public CqlException(string message) : base(message)
        {
        }

        public CqlException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

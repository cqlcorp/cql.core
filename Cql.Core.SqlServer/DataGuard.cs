using System;
using System.Collections.Generic;

namespace Cql.Core.SqlServer
{
    public static class DataGuard
    {
        public static void ExpectThatValueIsNotNull<T>(T value) where T : class
        {
            ExpectThatValueIsNotNull(null, value);
        }

        public static void ExpectThatValueIsNotNull<T>(string parameterName, T value) where T : class
        {
            if (value == null)
            {
                ThrowValueCannotBeNullException<T>(parameterName);
            }
        }

        public static void ExpectThatStringIsNotNullOrEmpty(string parameterName, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentOutOfRangeException($"The string parameter named '{parameterName}' cannot be null or empty.");
            }
        }

        public static void ExpectThatStringIsNotNullOrEmpty(string parameterName, string value, int maxLenth)
        {
            ExpectThatStringIsNotNullOrEmpty(parameterName, value);

            if (value.Length > maxLenth)
            {
                throw new ArgumentOutOfRangeException($"The string parameter named '{parameterName}' must be less than {maxLenth:n0} characters long.");
            }
        }

        public static void ExpectThatValueIsGreaterThanZero(string parameterName, long value)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException($"The parameter named '{parameterName}' must be greater than 0.");
            }
        }

        public static void ExpectThatValueIsNotEqualToDefault<T>(string parameterName, T value)
        {
            if (EqualityComparer<T>.Default.Equals(value, default(T)))
            {
                throw new ArgumentOutOfRangeException($"The value for the parameter named '{parameterName}' must not be equal to '{default(T)}'");
            }
        }

        private static void ThrowValueCannotBeNullException<T>(string parameterName)
        {
            string message = $"An argument of type '{typeof(T).FullName}' cannot be null";

            if (!string.IsNullOrEmpty(parameterName))
            {
                throw new ArgumentNullException(parameterName, message);
            }

            throw new ArgumentNullException("", message);
        }
    }
}

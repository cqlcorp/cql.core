// ***********************************************************************
// Assembly         : Cql.Core.SqlServer
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="DataGuard.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using JetBrains.Annotations;

    /// <summary>
    /// Class DataGuard.
    /// </summary>
    public static class DataGuard
    {
        /// <summary>
        /// Raises an <see cref="ArgumentOutOfRangeException"/> if the specified <paramref name="value"/> is null or empty.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="ArgumentOutOfRangeException">The string parameter named <paramref name="parameterName"/> cannot be null or empty.</exception>
        public static void ExpectThatStringIsNotNullOrEmpty([NotNull] string parameterName, [CanBeNull] string value)
        {
            Contract.Requires(!string.IsNullOrEmpty(parameterName));

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentOutOfRangeException($"The string parameter named '{parameterName}' cannot be null or empty.");
            }
        }

        /// <summary>
        /// Raises an <see cref="ArgumentOutOfRangeException"/> if the specified <paramref name="value"/> is null or empty or exceeds the specified <paramref name="maxLength"/>.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="value">The value.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <exception cref="ArgumentOutOfRangeException">The string parameter named <paramref name="parameterName"/> cannot be null or empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The string parameter named <paramref name="parameterName"/> must be less than <paramref name="maxLength"/> characters long.</exception>
        public static void ExpectThatStringIsNotNullOrEmpty([NotNull] string parameterName, [CanBeNull] string value, int maxLength)
        {
            Contract.Requires(!string.IsNullOrEmpty(parameterName));

            ExpectThatStringIsNotNullOrEmpty(parameterName, value);

            if (value?.Length > maxLength)
            {
                throw new ArgumentOutOfRangeException($"The string parameter named '{parameterName}' must be less than {maxLength:n0} characters long.");
            }
        }

        /// <summary>
        /// Raises an <see cref="ArgumentOutOfRangeException"/> if the specified <paramref name="value"/> is not greater than zero.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="ArgumentOutOfRangeException">The parameter named <paramref name="parameterName"/> must be greater than 0.</exception>
        public static void ExpectThatValueIsGreaterThanZero([NotNull] string parameterName, long value)
        {
            Contract.Requires(!string.IsNullOrEmpty(parameterName));

            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException($"The parameter named '{parameterName}' must be greater than 0.");
            }
        }

        /// <summary>
        /// Raises an <see cref="ArgumentOutOfRangeException"/> if the specified <paramref name="value"/> is equal to the default value of <typeparamref name="TValue"/>.
        /// </summary>
        /// <typeparam name="TValue">The type</typeparam>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="ArgumentOutOfRangeException">The value for the parameter named <paramref name="parameterName"/> must not be equal to '{default(T)}'</exception>
        public static void ExpectThatValueIsNotEqualToDefault<TValue>([NotNull] string parameterName, TValue value)
        {
            Contract.Requires(!string.IsNullOrEmpty(parameterName));

            if (EqualityComparer<TValue>.Default.Equals(value, default(TValue)))
            {
                throw new ArgumentOutOfRangeException($"The value for the parameter named '{parameterName}' must not be equal to '{default(TValue)}'");
            }
        }

        /// <summary>
        /// Raises an <see cref="ArgumentNullException"/> if the specified <paramref name="value"/> is null.
        /// </summary>
        /// <typeparam name="TValue">The value type</typeparam>
        /// <param name="value">The value.</param>
        /// <exception cref="ArgumentNullException">Argument cannot be null</exception>
        public static void ExpectThatValueIsNotNull<TValue>([CanBeNull] TValue value)
            where TValue : class
        {
            ExpectThatValueIsNotNull(null, value);
        }

        /// <summary>
        /// Expects the that value is not null.
        /// </summary>
        /// <typeparam name="TValue">The value type</typeparam>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="ArgumentNullException">Argument cannot be null</exception>
        public static void ExpectThatValueIsNotNull<TValue>([CanBeNull] string parameterName, [CanBeNull] TValue value)
            where TValue : class
        {
            if (value == null)
            {
                ThrowValueCannotBeNullException<TValue>(parameterName);
            }
        }

        /// <summary>
        /// Throws the value cannot be null exception.
        /// </summary>
        /// <typeparam name="TValue">The value type</typeparam>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Argument cannot be null</exception>
        private static void ThrowValueCannotBeNullException<TValue>([CanBeNull] string parameterName)
        {
            var message = $"An argument of type '{typeof(TValue).FullName}' cannot be null";

            if (string.IsNullOrEmpty(parameterName))
            {
                throw new ArgumentNullException(string.Empty, message);
            }

            throw new ArgumentNullException(parameterName, message);
        }
    }
}

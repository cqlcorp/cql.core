// ***********************************************************************
// Assembly         : Cql.Core.Common
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="StringExtensions.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Cql.Core.Common.Extensions
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;

    using JetBrains.Annotations;

    /// <summary>
    /// Class StringExtensions.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts the value to <typeparamref name="TValue"/>
        /// </summary>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>The converted value or default</returns>
        public static TValue As<TValue>([CanBeNull] this string value)
        {
            return As(value, default(TValue));
        }

        /// <summary>
        /// Converts the value to <typeparamref name="TValue"/>
        /// </summary>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The converted value or default</returns>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "We want to make this user friendly and return the default value on all failures")]
        public static TValue As<TValue>(this string value, TValue defaultValue)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(TValue));
                if (converter.CanConvertFrom(typeof(string)))
                {
                    return (TValue)converter.ConvertFrom(value);
                }

                // try the other direction
                converter = TypeDescriptor.GetConverter(typeof(string));
                if (converter.CanConvertTo(typeof(TValue)))
                {
                    return (TValue)converter.ConvertTo(value, typeof(TValue));
                }
            }
            catch
            {
                // eat all exceptions and return the defaultValue, assumption is that its always a parse/format exception
            }

            return defaultValue;
        }

        /// <summary>
        /// An overload of string contains that supports Case Insensitive comparison.
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <param name="value">The value.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <returns><c>true</c> if [contains] [the specified value]; otherwise, <c>false</c>.</returns>
        public static bool Contains(this string s, string value, StringComparison comparisonType)
        {
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }

            if (string.IsNullOrEmpty(value))
            {
                return true;
            }

            return s.IndexOf(value, comparisonType) >= 0;
        }

        /// <summary>
        /// Extends ToString formatting to Nullable types.
        /// </summary>
        /// <typeparam name="T">The current type must be a value type that implements IFormattable</typeparam>
        /// <param name="s">The s.</param>
        /// <param name="format">The format.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        [NotNull]
        public static string ToString<T>(this T? s, [NotNull] string format, [CanBeNull] IFormatProvider formatProvider = null)
            where T : struct, IFormattable
        {
            return s?.ToString(format, formatProvider) ?? string.Empty;
        }
    }
}

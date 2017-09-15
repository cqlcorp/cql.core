// ***********************************************************************
// Assembly         : Cql.Core.Common
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="RuntimeHelper.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Cql.Core.Common.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using JetBrains.Annotations;

    /// <summary>
    /// Class RuntimeHelper.
    /// </summary>
    public static class RuntimeHelper
    {
        /// <summary>
        /// Attempts the specified method and swallows any exceptions.
        /// </summary>
        /// <param name="method">The method.</param>
        public static void Attempt([CanBeNull] Action method)
        {
            try
            {
                method?.Invoke();
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// Attempts the specified task.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns>The attempted task.</returns>
        public static async Task Attempt([CanBeNull] Task method)
        {
            try
            {
                if (method != null)
                {
                    await method.ConfigureAwait(false);
                }
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// Always returns <c>false</c> at runtime
        /// </summary>
        /// <returns><c>false</c></returns>
        public static bool FalseAtRuntime()
        {
            return !TrueAtRuntime();
        }

        /// <summary>
        /// Returns <c>true</c> if <paramref name="value"/> is equal to the default value for type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the value is equal to the default; otherwise, <c>false</c>.</returns>
        public static bool IsDefaultValue<T>(this T value)
        {
            return EqualityComparer<T>.Default.Equals(value, default(T));
        }

        /// <summary>
        /// Always returns <c>true</c> at runtime
        /// </summary>
        /// <returns><c>true</c></returns>
        public static bool TrueAtRuntime() => int.Parse("1") == 1;
    }
}

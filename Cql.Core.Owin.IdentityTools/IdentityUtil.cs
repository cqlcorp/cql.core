// ***********************************************************************
// Assembly         : Cql.Core.Owin.IdentityTools
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="IdentityUtil.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.Owin.IdentityTools
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    using JetBrains.Annotations;

    /// <summary>
    /// Class IdentityUtil.
    /// </summary>
    public class IdentityUtil
    {
        /// <summary>
        /// Computes a SHA256 hash of the supplied value.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        ///     <see cref="System.String" />
        /// </returns>
        [NotNull]
        public static string ComputeHash([NotNull] string input)
        {
            Contract.Requires(!string.IsNullOrEmpty(input));

            using (HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider())
            {
                try
                {
                    var byteValue = Encoding.UTF8.GetBytes(input);

                    var byteHash = hashAlgorithm.ComputeHash(byteValue);

                    return Convert.ToBase64String(byteHash);
                }
                finally
                {
                    hashAlgorithm.Clear();
                }
            }
        }

        /// <summary>
        /// Generates a random character ID values from <paramref name="size" /> iterations of <see cref="Guid" /> string values.
        /// <para></para>
        /// <para>Example Outputs:</para>
        /// <para>size = 1 -&gt; 837d3b4a0f3e4dd7909839844a6f4f8b</para>
        /// <para>size = 2 -&gt; 156a433f14434651939f85e27af0d2a317e7c28b89d643fa9a9de99a546fb057</para>
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>
        ///     <see cref="System.String" />
        /// </returns>
        [NotNull]
        public static string NewId(int? size = null)
        {
            return string.Join(string.Empty, Enumerable.Repeat($"{Guid.NewGuid():n}", size.GetValueOrDefault(1)));
        }
    }
}

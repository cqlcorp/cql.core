// ***********************************************************************
// Assembly         : Cql.Core.Web
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="BasicAuthCredentials.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.Web
{
    using System.Net;
    using System.Net.Http.Headers;

    using JetBrains.Annotations;

    /// <summary>
    /// Class BasicAuthCredentials.
    /// </summary>
    /// <seealso cref="Cql.Core.Web.IBasicAuthCredentials" />
    public class BasicAuthCredentials : IBasicAuthCredentials
    {
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; set; }

        /// <summary>
        /// Performs an implicit conversion from <see cref="NetworkCredential" /> to <see cref="BasicAuthCredentials" />.
        /// </summary>
        /// <param name="networkCredential">The network credential.</param>
        /// <returns>The result of the conversion.</returns>
        [NotNull]
        public static implicit operator BasicAuthCredentials([CanBeNull] NetworkCredential networkCredential)
        {
            return new BasicAuthCredentials { UserName = networkCredential?.UserName, Password = networkCredential?.Password };
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="BasicAuthCredentials" /> to <see cref="AuthenticationHeaderValue" />.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <returns>An <see cref="AuthenticationHeaderValue"/> containing the specified <paramref name="credentials"/>.</returns>
        [NotNull]
        public static implicit operator AuthenticationHeaderValue([NotNull] BasicAuthCredentials credentials)
        {
            return BasicAuthenticationHelper.CreateBasicAuthHeader(credentials);
        }
    }
}

// ***********************************************************************
// Assembly         : Cql.Core.Web
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-15-2017
// ***********************************************************************
// <copyright file="IBasicAuthCredentials.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.Web
{
    /// <summary>
    /// Interface IBasicAuthCredentials
    /// </summary>
    public interface IBasicAuthCredentials
    {
        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <value>The password.</value>
        string Password { get; }

        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        string UserName { get; }
    }
}

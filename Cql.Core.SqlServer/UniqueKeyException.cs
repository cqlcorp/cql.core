// ***********************************************************************
// Assembly         : Cql.Core.SqlServer
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="UniqueKeyException.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.SqlServer
{
    using System.Data.Common;

    using JetBrains.Annotations;

    /// <summary>
    /// Class UniqueKeyException.
    /// </summary>
    /// <seealso cref="System.Data.Common.DbException" />
    public class UniqueKeyException : DbException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueKeyException"/> class.
        /// </summary>
        /// <param name="message">The message to display for this exception.</param>
        public UniqueKeyException([NotNull] string message)
            : base(message)
        {
        }
    }
}

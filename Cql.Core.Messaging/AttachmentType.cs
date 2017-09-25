// ***********************************************************************
// Assembly         : Cql.Core.Messaging
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-15-2017
// ***********************************************************************
// <copyright file="AttachmentType.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.Messaging
{
    /// <summary>
    /// Enum AttachmentType
    /// </summary>
    public enum AttachmentType
    {
        /// <summary>
        /// Not specified.
        /// </summary>
        None = 0,

        /// <summary>
        /// The file path
        /// </summary>
        FilePath = 1,

        /// <summary>
        /// The stream
        /// </summary>
        Stream = 2
    }
}

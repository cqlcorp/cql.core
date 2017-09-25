// ***********************************************************************
// Assembly         : Cql.Core.Messaging
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-15-2017
// ***********************************************************************
// <copyright file="DispatchRule.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.Messaging
{
    /// <summary>
    /// Enum DispatchMode
    /// </summary>
    public enum DispatchMode
    {
        /// <summary>
        /// Prevent dispatching to live service (for development and testing).
        /// </summary>
        Test = 0,

        /// <summary>
        /// Dispatches messages to the live service (production mode).
        /// </summary>
        Production = 1
    }
}

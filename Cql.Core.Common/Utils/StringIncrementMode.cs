// ***********************************************************************
// Assembly         : Cql.Core.Common
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="StringIncrementMode.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Cql.Core.Common.Utils
{
    /// <summary>
    /// Determines the implementation of the StringIncrement method.
    /// </summary>
    public enum StringIncrementMode
    {
        /// <summary>
        /// Increments the last character using alpha or numeric characters.
        /// </summary>
        AlphaNumeric = 1,

        /// <summary>
        /// Increments the last character using alphabet characters.
        /// </summary>
        Alpha = 2,

        /// <summary>
        /// Increments the last character using numeric characters.
        /// </summary>
        Numeric = 3,

        /// <summary>
        /// Increases the value so that it is sequentially greater.
        /// </summary>
        Sequence = 4
    }
}

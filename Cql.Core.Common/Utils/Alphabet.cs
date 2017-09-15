// ***********************************************************************
// Assembly         : Cql.Core.Common
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="Alphabet.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.Common.Utils
{
    using JetBrains.Annotations;

    /// <summary>
    /// Class Alphabet.
    /// </summary>
    public static class Alphabet
    {
        /// <summary>
        /// The lower case alphabet
        /// </summary>
        [NotNull]
        public static readonly string[] LowerCase =
            { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

        /// <summary>
        /// The upper case alphabet
        /// </summary>
        [NotNull]
        public static readonly string[] UpperCase =
            { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
    }
}

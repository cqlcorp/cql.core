// ***********************************************************************
// Assembly         : Cql.Core.Common
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-15-2017
// ***********************************************************************
// <copyright file="DataValueAttribute.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.Common.Attributes
{
    using System;

    using JetBrains.Annotations;

    /// <summary>
    /// Class DataValueAttribute. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.All)]
    public sealed class DataValueAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataValueAttribute" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <exception cref="ArgumentNullException">The value cannot be null</exception>
        public DataValueAttribute([NotNull] string value)
        {
            this.Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        [NotNull]
        public string Value { get; }
    }
}

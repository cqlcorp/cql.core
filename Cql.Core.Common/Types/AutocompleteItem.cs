// ***********************************************************************
// Assembly         : Cql.Core.Common
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="AutocompleteItem.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Cql.Core.Common.Types
{
    using System;

    /// <summary>
    /// Class AutocompleteItem.
    /// </summary>
    public class AutocompleteItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutocompleteItem"/> class.
        /// </summary>
        public AutocompleteItem()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutocompleteItem"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="text">The text.</param>
        public AutocompleteItem(object id, object text)
            : this(Convert.ToString(id), Convert.ToString(text))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutocompleteItem"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public AutocompleteItem(string text)
            : this(text, text)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutocompleteItem"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="text">The text.</param>
        public AutocompleteItem(string id, string text)
        {
            this.Id = id;
            this.Text = text;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }
    }
}

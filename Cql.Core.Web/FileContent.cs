// ***********************************************************************
// Assembly         : Cql.Core.Web
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-15-2017
// ***********************************************************************
// <copyright file="FileContent.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.Web
{
    /// <summary>
    /// Class FileContent.
    /// </summary>
    public class FileContent
    {
        /// <summary>
        /// Gets or sets the content bytes.
        /// </summary>
        /// <value>The content bytes.</value>
        public byte[] ContentBytes { get; set; }

        /// <summary>
        /// Gets or sets the content delivery option.
        /// </summary>
        /// <value>The content delivery.</value>
        public ContentDelivery? ContentDelivery { get; set; }

        /// <summary>
        /// Gets or sets the content type.
        /// </summary>
        /// <value>The type of the content.</value>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public string FileName { get; set; }

        /// <summary>
        /// Gets a value indicating whether file content was not found.
        /// </summary>
        /// <value><c>true</c> if the <see cref="ContentBytes"/> are null or 0 length; otherwise, <c>false</c>.</value>
        public bool NotFound => this.ContentBytes == null || this.ContentBytes.Length == 0;
    }
}

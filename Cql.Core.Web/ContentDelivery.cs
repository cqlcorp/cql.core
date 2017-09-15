// ***********************************************************************
// Assembly         : Cql.Core.Web
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-15-2017
// ***********************************************************************
// <copyright file="ContentDelivery.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.Web
{
    /// <summary>
    /// Used in determining the Content-Disposition header when writing the response.
    /// </summary>
    public enum ContentDelivery
    {
        /// <summary>
        /// Content is rendered directly to the browser window.
        /// </summary>
        Inline = 0,

        /// <summary>
        /// Content is sent as a "Download/Save As" attachment to the browser.
        /// </summary>
        Attachment = 1
    }
}

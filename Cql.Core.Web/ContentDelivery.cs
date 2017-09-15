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

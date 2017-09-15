namespace Cql.Core.Web
{
    public enum UseSslOption
    {
        /// <summary>
        /// Generates a link that matches the current request.
        /// </summary>
        UseCurrentRequest = 0,

        /// <summary>
        /// Generates a link that always uses HTTP traffic.
        /// </summary>
        Never = 1,

        /// <summary>
        /// Generates a link that always uses SSL traffic.
        /// </summary>
        Always = 2
    }
}

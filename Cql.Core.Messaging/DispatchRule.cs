namespace Cql.Core.Messaging
{
    public enum DispatchMode
    {
        /// <summary>
        /// Prevent dispatching to live service (for development and testing).
        /// </summary>
        Test = 0,

        /// <summary>
        /// Dispatches messages to the live service (production mode).
        /// </summary>
        Production
    }
}

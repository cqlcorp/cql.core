namespace Cql.Core.Owin
{
    using System;
    using System.IO;

    public static class AppUtil
    {
        /// <summary>
        /// Returns the DataDirectory path for the <see cref="AppDomain" />.
        /// </summary>
        /// <returns></returns>
        public static string GetAppDataPath()
        {
            var appDataPath = AppDomain.CurrentDomain.GetData("DataDirectory")?.ToString();

            if (string.IsNullOrEmpty(appDataPath))
            {
                throw new InvalidOperationException("The current AppDomain does not have a DataDirectory value available.");
            }

            var dInfo = new DirectoryInfo(appDataPath);

            if (!dInfo.Exists)
            {
                dInfo.Create();
            }

            return dInfo.FullName;
        }
    }
}

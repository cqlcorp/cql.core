namespace Cql.Core.Owin
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Mail;
    using System.Text.RegularExpressions;

    public static class WebUtil
    {
        [SuppressMessage("ReSharper", "InvertIf")]
        public static string GetSubDomain(Uri url)
        {
            if (url.HostNameType == UriHostNameType.Dns)
            {
                var host = url.Host;

                return GetSubDomain(host);
            }

            return null;
        }

        [SuppressMessage("ReSharper", "StringLastIndexOfIsCultureSpecific.1")]
        [SuppressMessage("ReSharper", "StringLastIndexOfIsCultureSpecific.2")]
        public static string GetSubDomain(string host)
        {
            if (host.Split('.').Length <= 2)
            {
                return null;
            }

            var lastIndex = host.LastIndexOf(".");
            var index = host.LastIndexOf(".", lastIndex - 1);

            return host.Substring(0, index);
        }

        /// <summary>
        /// Checks to see if the email is valid
        /// </summary>
        /// <param name="address">The email address</param>
        /// <returns>
        ///     <c>true</c>
        /// </returns>
        public static bool IsEmailValid(string address)
        {
            try
            {
                ValidateEmail(address);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static MailAddressCollection ParseEmailAddresses(string value)
        {
            var addressCollection = new MailAddressCollection();

            if (string.IsNullOrWhiteSpace(value))
            {
                return addressCollection;
            }

            if (Regex.IsMatch(value, ";|,"))
            {
                var addresses = value.Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);

                for (var i = 0; i <= addresses.GetUpperBound(0); i++)
                {
                    var address = addresses[i];

                    if (!string.IsNullOrEmpty(address))
                    {
                        addressCollection.Add(address);
                    }
                }
            }
            else
            {
                addressCollection.Add(value);
            }

            return addressCollection;
        }

        /// <summary>
        /// Validates the email
        /// </summary>
        /// <param name="address"></param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="address" /> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="address" /> is <see cref="F:System.String.Empty" /> ("").
        /// </exception>
        /// <exception cref="T:System.FormatException">
        /// <paramref name="address" /> is not in a recognized format.
        /// </exception>
        public static void ValidateEmail(string address)
        {
            // ReSharper disable once ObjectCreationAsStatement
            new MailAddress(address);
        }
    }
}

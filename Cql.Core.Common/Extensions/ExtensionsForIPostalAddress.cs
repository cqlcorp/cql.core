// ***********************************************************************
// Assembly         : Cql.Core.Common
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="ExtensionsForIPostalAddress.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Cql.Core.Common.Extensions
{
    using System.Text;

    using Cql.Core.Common.Types;

    using JetBrains.Annotations;

    /// <summary>
    /// Class ExtensionsForIPostalAddress.
    /// </summary>
    public static class ExtensionsForIPostalAddress
    {
        /// <summary>
        /// Returns a formatted delivery line by combining the Address1 and Address2 properties.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>Returns a value similar to &quot;123 E Main St APT #7&quot;</returns>
        [NotNull]
        public static string ToAddressDeliveryLine([CanBeNull] this IPostalAddress address)
        {
            return $"{address?.Address1} {address?.Address2}".Trim();
        }

        /// <summary>
        /// Returns a formatted last line of a delivery address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>Returns a value similar to &quot;Grand Rapids, MI 49505 USA&quot;</returns>
        public static string ToAddressLastLine(this IPostalAddress address)
        {
            var sb = new StringBuilder();

            var includesCity = false;
            var includesState = false;
            var includesPostalCode = false;

            if (!string.IsNullOrEmpty(address?.City))
            {
                sb.Append(address.City);
                includesCity = true;
            }

            if (!string.IsNullOrEmpty(address?.State))
            {
                if (includesCity)
                {
                    sb.Append(", ");
                }

                sb.Append(address.State);
                includesState = true;
            }

            if (!string.IsNullOrEmpty(address?.Zip))
            {
                if (includesState || includesCity)
                {
                    sb.Append(" ");
                }

                sb.Append(address.Zip);
                includesPostalCode = true;
            }

            if (!string.IsNullOrEmpty(address?.Country))
            {
                if (includesState || includesCity || includesPostalCode)
                {
                    sb.Append(" ");
                }

                sb.Append(address.Country);
            }

            return sb.ToString();
        }
    }
}

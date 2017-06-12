using System.Collections.Generic;
using System.Text;
using Cql.Core.Common.Types;

namespace Cql.Core.Common.Extensions
{
    public static class ExtensionsForIPostalAddress
    {
        public static string ToAddressDeliveryLine(this IPostalAddress address)
        {
            var elements = new List<string>
                           {
                               address?.Address1,
                               address?.Address2
                           };

            return string.Join(" ", elements).Trim();
        }

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

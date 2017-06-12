using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Cql.Core.Owin.IdentityTools
{
    public class IdentityUtil
    {
        /// <summary>
        /// Computes a SHA256 hash of the supplied value.
        /// </summary>
        public static string ComputeHash(string input)
        {
            using (HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider())
            {
                try
                {
                    var byteValue = Encoding.UTF8.GetBytes(input);

                    var byteHash = hashAlgorithm.ComputeHash(byteValue);

                    return Convert.ToBase64String(byteHash);
                }
                finally
                {
                    hashAlgorithm.Clear();
                }
            }
        }

        /// <summary>
        /// Generates a random character ID values from <paramref name="size"/> iterations of <see cref="Guid"/> string values.
        /// <para> </para>
        /// <para>Example Outputs:</para>
        /// <para>size = 1 -> 837d3b4a0f3e4dd7909839844a6f4f8b</para>
        /// <para>size = 2 -> 156a433f14434651939f85e27af0d2a317e7c28b89d643fa9a9de99a546fb057</para>
        /// </summary>
        public static string NewId(int? size = null)
        {
            return string.Join("", Enumerable.Repeat($"{Guid.NewGuid():n}", size.GetValueOrDefault(1)));
        }
    }
}

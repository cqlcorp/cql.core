namespace Cql.Core.Owin.IdentityTools
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;

    using Cql.Core.Common.Extensions;

    using Newtonsoft.Json;

    public static class ExtensionsForClaims
    {
        public static bool? GetBoolClaimValue(this IIdentity identity, Enum claimType)
        {
            return GetBoolClaimValue(identity, claimType.GetDataValue());
        }

        public static bool? GetBoolClaimValue(this IIdentity identity, string claimType)
        {
            var claimsIdentity = identity as ClaimsIdentity;

            var value = claimsIdentity?.Claims.GetClaimValue(claimType);

            bool result;

            return bool.TryParse(value, out result) ? result : default(bool?);
        }

        public static TValue GetClaimJsonValue<TValue>(this IIdentity identity, Enum claimType, TValue defaultValue = default(TValue))
        {
            return GetClaimJsonValue(identity, claimType.GetDataValue(), defaultValue);
        }

        public static TValue GetClaimJsonValue<TValue>(this IIdentity identity, string claimType, TValue defaultValue = default(TValue))
        {
            var json = identity.GetClaimValue(claimType);

            return string.IsNullOrWhiteSpace(json) ? defaultValue : JsonConvert.DeserializeObject<TValue>(json);
        }

        public static string GetClaimValue(this IEnumerable<Claim> claims, Enum claimType)
        {
            return claims.GetClaimValue(claimType.GetDataValue());
        }

        public static string GetClaimValue(this IEnumerable<Claim> claims, string claimType)
        {
            return claims?.Where(c => c.Type == claimType).Select(c => c.Value).FirstOrDefault();
        }

        public static string GetClaimValue(this IIdentity identity, Enum claimType)
        {
            return GetClaimValue(identity, claimType.GetDataValue());
        }

        public static string GetClaimValue(this IIdentity identity, string claimType)
        {
            var claimsIdentity = identity as ClaimsIdentity;

            return claimsIdentity?.Claims.GetClaimValue(claimType);
        }

        public static double? GetDoubleClaimValue(this IIdentity identity, Enum claimType)
        {
            return GetDoubleClaimValue(identity, claimType.GetDataValue());
        }

        public static double? GetDoubleClaimValue(this IIdentity identity, string claimType)
        {
            var claimsIdentity = identity as ClaimsIdentity;

            var value = claimsIdentity?.Claims.GetClaimValue(claimType);

            double result;

            return double.TryParse(value, out result) ? result : default(double?);
        }

        public static Guid? GetGuidClaimValue(this IIdentity identity, Enum claimType)
        {
            return GetGuidClaimValue(identity, claimType.GetDataValue());
        }

        public static Guid? GetGuidClaimValue(this IIdentity identity, string claimType)
        {
            var claimsIdentity = identity as ClaimsIdentity;

            var value = claimsIdentity?.Claims.GetClaimValue(claimType);

            Guid result;

            return Guid.TryParse(value, out result) ? result : default(Guid?);
        }

        public static int? GetIntClaimValue(this IIdentity identity, Enum claimType)
        {
            return GetIntClaimValue(identity, claimType.GetDataValue());
        }

        public static int? GetIntClaimValue(this IIdentity identity, string claimType)
        {
            var claimsIdentity = identity as ClaimsIdentity;

            var value = claimsIdentity?.Claims.GetClaimValue(claimType);

            int result;

            return int.TryParse(value, out result) ? result : default(int?);
        }

        public static IEnumerable<string> GetRoleNames(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;

            return claimsIdentity == null ? Enumerable.Empty<string>() : claimsIdentity.Claims.GetRoleNames();
        }

        public static IEnumerable<string> GetRoleNames(this IEnumerable<Claim> claims)
        {
            return claims?.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value) ?? new string[] { };
        }

        public static IEnumerable<TRolesEnum> GetRoles<TRolesEnum>(this IEnumerable<Claim> claims)
            where TRolesEnum : struct, IConvertible
        {
            return claims?.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value.FromDescription<TRolesEnum>()) ?? new TRolesEnum[] { };
        }
    }
}

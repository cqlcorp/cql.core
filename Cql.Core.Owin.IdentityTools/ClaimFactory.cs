namespace Cql.Core.Owin.IdentityTools
{
    using System;
    using System.Security.Claims;

    using Cql.Core.Common.Extensions;

    using Newtonsoft.Json;

    public static class ClaimFactory
    {
        public static Claim Create(Enum claimType, Enum value)
        {
            return new Claim(GetClaimTypeName(claimType), value.GetDataValue());
        }

        public static Claim Create(Enum claimType, object value)
        {
            return new Claim(GetClaimTypeName(claimType), Convert.ToString(value));
        }

        public static Claim Create(string claimType, object value)
        {
            return new Claim(claimType, Convert.ToString(value));
        }

        public static Claim CreateJsonClaim(string claimType, object value)
        {
            return new Claim(claimType, JsonConvert.SerializeObject(value));
        }

        public static Claim CreateJsonClaim(Enum claimType, object value)
        {
            return new Claim(GetClaimTypeName(claimType), JsonConvert.SerializeObject(value));
        }

        public static string GetClaimTypeName(Enum value)
        {
            return value.GetDataValue();
        }
    }
}

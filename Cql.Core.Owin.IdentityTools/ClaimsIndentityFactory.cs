using System;
using System.Security.Claims;

using JetBrains.Annotations;

namespace Cql.Core.Owin.IdentityTools
{
    public static class ClaimsIndentityFactory
    {
        private const string IdentityProviderClaimType = "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider";
        private const string DefaultIdentityProviderClaimValue = "ASP.NET Identity";
        private const string RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
        private const string UserIdClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        private const string UserNameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
        private const string StringClaimValue = "http://www.w3.org/2001/XMLSchema#string";

        /// <summary>
        /// Creates an instance of a <see cref="ClaimsIdentity" />
        /// </summary>
        /// <param name="createOptions">The create options.</param>
        /// <returns>ClaimsIdentity.</returns>
        /// <exception cref="ArgumentNullException">createOptions may not bell null</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// AuthenticationType and UserId may not be null or empty.
        /// </exception>
        public static ClaimsIdentity CreateIdentity([NotNull] IdentityCreateOptions createOptions)
        {
            if (createOptions == null)
            {
                throw new ArgumentNullException(nameof(createOptions));
            }

            if (string.IsNullOrEmpty(createOptions.AuthenticationType))
            {
                throw new ArgumentOutOfRangeException(nameof(createOptions.AuthenticationType));
            }


            if (string.IsNullOrEmpty(createOptions.UserId))
            {
                throw new ArgumentOutOfRangeException(nameof(createOptions.UserId));
            }

            if (string.IsNullOrEmpty(createOptions.IdentityProviderName))
            {
                createOptions.IdentityProviderName = DefaultIdentityProviderClaimValue;
            }

            var id = new ClaimsIdentity(createOptions.AuthenticationType, UserNameClaimType, RoleClaimType);

            id.AddClaim(new Claim(UserIdClaimType, createOptions.UserId, StringClaimValue));
            id.AddClaim(new Claim(UserNameClaimType, createOptions.UserName, StringClaimValue));
            id.AddClaim(new Claim(IdentityProviderClaimType, createOptions.IdentityProviderName, StringClaimValue));

            foreach (var role in createOptions.Roles)
            {
                id.AddClaim(new Claim(RoleClaimType, role, StringClaimValue));
            }

            foreach (var claim in createOptions.AdditionalClaims)
            {
                id.AddClaim(new Claim(claim.Key, claim.Value));
            }

            return id;
        }
    }
}

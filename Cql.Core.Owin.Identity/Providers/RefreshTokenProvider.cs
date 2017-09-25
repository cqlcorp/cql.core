// ***********************************************************************
// Assembly         : Cql.Core.Owin.Identity
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="RefreshTokenProvider.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Cql.Core.Owin.Identity.Providers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;

    using Cql.Core.Owin.Identity.Repositories;
    using Cql.Core.Owin.Identity.Types;
    using Cql.Core.Owin.IdentityTools;
    using Cql.Core.ServiceLocation;

    using JetBrains.Annotations;

    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Infrastructure;

    /// <summary>
    /// Class RefreshTokenProvider.
    /// </summary>
    /// <seealso cref="Microsoft.Owin.Security.Infrastructure.AuthenticationTokenProvider" />
    public class RefreshTokenProvider : AuthenticationTokenProvider
    {
        /// <summary>
        /// Creates a new refesh token.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns><see cref="Task"/></returns>
        public override async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var ticket = context.Ticket;

            var clientId = ticket.Properties.Dictionary[".client_id"];

            if (string.IsNullOrEmpty(clientId))
            {
                return;
            }

            var refreshTokenId = IdentityUtil.NewId(2);

            var refreshTokenLifeTime = context.OwinContext.Get<string>(OwinKeys.ClientRefreshTokenLifeTime);

            var token = CreateRefreshToken(ticket, refreshTokenId, clientId, Convert.ToDouble(refreshTokenLifeTime));

            token.ProtectedTicket = context.SerializeTicket();

            var identityStore = GetIdentityStore();

            var result = await identityStore.AddRefreshToken(token);

            if (result)
            {
                context.SetToken(refreshTokenId);
            }
        }

        /// <summary>
        /// Receives the Refresh Token from the "refresh_token" grant.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns><see cref="Task"/></returns>
        public override async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var owinContext = context.OwinContext;

            var allowedOrigin = owinContext.Get<string>(OwinKeys.ClientAllowedOrigin) ?? "*";

            owinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            var hashedTokenId = IdentityUtil.ComputeHash(context.Token);

            var identityStore = GetIdentityStore();

            var refreshToken = await identityStore.FindRefreshToken(hashedTokenId);

            if (refreshToken != null)
            {
                context.DeserializeTicket(refreshToken.ProtectedTicket);

                await identityStore.RemoveRefreshToken(hashedTokenId);
            }
        }

        /// <summary>
        /// Creates the refresh token.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="refreshTokenId">The refresh token identifier.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="expiresInMinutes">The expires in minutes.</param>
        /// <returns><see cref="RefreshToken"/></returns>
        [NotNull]
        private static RefreshToken CreateRefreshToken([NotNull] AuthenticationTicket ticket, [NotNull] string refreshTokenId, [NotNull] string clientId, double expiresInMinutes)
        {
            Contract.Requires(!string.IsNullOrEmpty(refreshTokenId));
            Contract.Requires(!string.IsNullOrEmpty(clientId));

            var token = new RefreshToken
                            {
                                Id = IdentityUtil.ComputeHash(refreshTokenId),
                                ClientId = clientId,
                                Subject = ticket.Identity.Name,
                                IssuedUtc = DateTime.UtcNow,
                                ExpiresUtc = DateTime.UtcNow.AddMinutes(expiresInMinutes)
                            };

            var props = ticket.Properties;

            props.IssuedUtc = token.IssuedUtc;
            props.ExpiresUtc = token.ExpiresUtc;

            return token;
        }

        /// <summary>
        /// Gets the identity store.
        /// </summary>
        /// <returns><see cref="IIdentityStore"/></returns>
        [NotNull]
        private static IIdentityStore GetIdentityStore()
        {
            return ServiceResolver.Resolve<IIdentityStore>();
        }
    }
}

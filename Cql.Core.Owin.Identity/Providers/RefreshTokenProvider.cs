using System;
using System.Threading.Tasks;

using Cql.Core.Owin.Identity.Repositories;
using Cql.Core.Owin.Identity.Types;
using Cql.Core.Owin.IdentityTools;
using Cql.Core.ServiceLocation;

using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;

namespace Cql.Core.Owin.Identity.Providers
{
    public class RefreshTokenProvider : AuthenticationTokenProvider
    {
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

        public override async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var owinContext = context.OwinContext;

            var allowedOrigin = owinContext.Get<string>(OwinKeys.ClientAllowedOrigin) ?? "*";

            owinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] {allowedOrigin});

            var hashedTokenId = IdentityUtil.ComputeHash(context.Token);

            var identityStore = GetIdentityStore();

            var refreshToken = await identityStore.FindRefreshToken(hashedTokenId);

            if (refreshToken != null)
            {
                context.DeserializeTicket(refreshToken.ProtectedTicket);

                await identityStore.RemoveRefreshToken(hashedTokenId);
            }
        }

        private static IIdentityStore GetIdentityStore()
        {
            return ServiceResolver.Resolve<IIdentityStore>();
        }

        private static RefreshToken CreateRefreshToken(AuthenticationTicket ticket, string refreshTokenId, string clientId, double expiresInMinutes)
        {
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
    }
}

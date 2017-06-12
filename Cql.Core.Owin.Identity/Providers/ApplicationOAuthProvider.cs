using System;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

using Cql.Core.Owin.Identity.Repositories;
using Cql.Core.Owin.Identity.Types;
using Cql.Core.ServiceLocation;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace Cql.Core.Owin.Identity.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        /// <summary>
        /// Called when a request to the Token endpoint arrives with a "grant_type" of "refresh_token". This occurs if your application has issued a "refresh_token"
        /// along with the "access_token", and the client is attempting to use the "refresh_token" to acquire a new "access_token", and possibly a new "refresh_token".
        /// To issue a refresh token the an Options.RefreshTokenProvider must be assigned to create the value which is returned. The claims and properties
        /// associated with the refresh token are present in the context.Ticket. The application must call context.Validated to instruct the
        /// Authorization Server middleware to issue an access token based on those claims and properties. The call to context.Validated may
        /// be given a different AuthenticationTicket or ClaimsIdentity in order to control which information flows from the refresh token to
        /// the access token. The default behavior when using the OAuthAuthorizationServerProvider is to flow information from the refresh token to
        /// the access token unmodified.
        /// See also http://tools.ietf.org/html/rfc6749#section-6
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>Task to enable asynchronous execution</returns>
        public override async Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary[".client_id"];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                return;
            }

            var applicationUserManager = context.OwinContext.Get<ApplicationUserManager>();

            var identityUser = await FindUserFromTicket(applicationUserManager, context);

            if (identityUser.AccessRevokedDate != null)
            {
                context.SetError("invalid_grant", "User access is revoked.");
                return;
            }

            var newIdentity = await ProviderUtils.GenerateUserIdentityAsync(applicationUserManager, identityUser);

            var authenticationProperties = GetAuthenticationProperties(context.ClientId, newIdentity, identityUser);

            var newTicket = new AuthenticationTicket(newIdentity, authenticationProperties);

            await UpdateLastActivity(identityUser.Id);

            context.Validated(newTicket);
        }

        /// <summary>
        /// Called when a request to the Token endpoint arrives with a "grant_type" of "password". This occurs when the user has provided name and password
        /// credentials directly into the client application's user interface, and the client application is using those to acquire an "access_token" and
        /// optional "refresh_token". If the web application supports the
        /// resource owner credentials grant type it must validate the context.Username and context.Password as appropriate. To issue an
        /// access token the context.Validated must be called with a new ticket containing the claims about the resource owner which should be associated
        /// with the access token. The application should take appropriate measures to ensure that the endpoint isn’t abused by malicious callers.
        /// The default behavior is to reject this grant type.
        /// See also http://tools.ietf.org/html/rfc6749#section-4.3.2
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>Task to enable asynchronous execution</returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>(OwinKeys.ClientAllowedOrigin) ?? "*";

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            var userManager = context.OwinContext.Get<ApplicationUserManager>();

            var user = await FindUserAsync(userManager, context);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            if (user.AccessRevokedDate != null)
            {
                context.SetError("invalid_grant", "User access is revoked.");
                return;
            }

            var claimsIdentity = await ProviderUtils.GenerateUserIdentityAsync(userManager, user);

            var authenticationProperties = GetAuthenticationProperties(context.ClientId, claimsIdentity, user);

            var ticket = new AuthenticationTicket(claimsIdentity, authenticationProperties);

            await UpdateLastActivity(user.Id);

            context.Validated(ticket);

            context.OwinContext.Set(OwinKeys.ClientId, context.ClientId);
        }

        /// <summary>
        /// Called at the final stage of a successful Token endpoint request. An application may implement this call in order to do any final
        /// modification of the claims being used to issue access or refresh tokens. This call may also be used in order to add additional
        /// response parameters to the Token endpoint's json response body.
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>Task to enable asynchronous execution</returns>
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (var property in context.Properties.Dictionary.Where(x => !x.Key.StartsWith(".")))
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Called to validate that the origin of the request is a registered "client_id", and that the correct credentials for that client are
        /// present on the request. If the web application accepts Basic authentication credentials,
        /// context.TryGetBasicCredentials(out clientId, out clientSecret) may be called to acquire those values if present in the request header. If the web
        /// application accepts "client_id" and "client_secret" as form encoded POST parameters,
        /// context.TryGetFormCredentials(out clientId, out clientSecret) may be called to acquire those values if present in the request body.
        /// If context.Validated is not called the request will not proceed further.
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>Task to enable asynchronous execution</returns>
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId;
            string clientSecret;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (string.IsNullOrEmpty(clientId))
            {
                context.SetError("invalid_clientId", "Client ID should be supplied.");
                return;
            }

            var clientRepository = ServiceResolver.Resolve<IClientRepository>();

            var client = await clientRepository.FindClientByIdAsync(clientId);

            if (client == null)
            {
                context.SetError("invalid_clientId", $"Client '{clientId}' is not registered in the system.");
                return;
            }

            if (!client.Active)
            {
                context.SetError("invalid_clientId", "Client is inactive.");
                return;
            }

            if (IsInvalidClient(context, client, clientSecret))
            {
                return;
            }

            context.OwinContext.Set(OwinKeys.ClientAllowedOrigin, client.AllowedOrigin);
            context.OwinContext.Set(OwinKeys.ClientRefreshTokenLifeTime, Convert.ToString(client.RefreshTokenLifeTime));

            context.Validated(clientId);
        }

        /// <summary>
        /// Called to validate that the context.ClientId is a registered "client_id", and that the context.RedirectUri a "redirect_uri"
        /// registered for that client. This only occurs when processing the Authorize endpoint. The application MUST implement this
        /// call, and it MUST validate both of those factors before calling context.Validated. If the context.Validated method is called
        /// with a given redirectUri parameter, then IsValidated will only become true if the incoming redirect URI matches the given redirect URI.
        /// If context.Validated is not called the request will not proceed further.
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>Task to enable asynchronous execution</returns>
        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            var expectedRootUri = new Uri(context.Request.Uri, "/");

            if (expectedRootUri.AbsoluteUri == context.RedirectUri)
            {
                context.Validated();
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Finds the user by validating the supplied credentials (typically Username and Password)
        /// <para></para>
        /// <para>return userManager.FindAsync(context.UserName, context.Password);</para>
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="context"></param>
        /// <returns>
        /// The identity user if the supplied credential match the user; otherwise <c>null</c>.
        /// </returns>
        protected virtual Task<IdentityUser> FindUserAsync(ApplicationUserManager userManager, OAuthGrantResourceOwnerCredentialsContext context)
        {
            return userManager.FindAsync(context.UserName, context.Password);
        }

        /// <summary>
        /// Finds the user by using information stored in the authorization ticket (typically UserID or Username).
        /// <para></para>
        /// <para>userManager.FindByIdAsync(context.Ticket.Identity.GetUserId$lt;int&gt;());</para>
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="context"></param>
        /// <returns>
        /// The identity user if the supplied ticket information matches the user; otherwise <c>null</c>.
        /// </returns>
        protected virtual Task<IdentityUser> FindUserFromTicket(ApplicationUserManager userManager, OAuthGrantRefreshTokenContext context)
        {
            return userManager.FindByIdAsync(context.Ticket.Identity.GetUserId<int>());
        }

        protected virtual AuthenticationProperties GetAuthenticationProperties(string clientId, IIdentity claimsIdentity, IdentityUser user)
        {
            return AuthenticationPropertyManager.GenerateAuthenticationProperties(claimsIdentity, user, clientId);
        }

        protected virtual bool IsInvalidClient(OAuthValidateClientAuthenticationContext context, Client client, string clientSecret)
        {
            if (client.ApplicationType == ApplicationType.Public)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(clientSecret))
            {
                context.SetError("invalid_clientId", "Client secret should be sent.");
                return true;
            }

            if (client.Secret != clientSecret)
            {
                context.SetError("invalid_clientId", "Client secret is invalid.");
                return true;
            }

            return false;
        }

        private static Task UpdateLastActivity(int userId)
        {
            return ServiceResolver.Resolve<IUserActivityRepository>().UpdateLastActivity(userId);
        }
    }
}

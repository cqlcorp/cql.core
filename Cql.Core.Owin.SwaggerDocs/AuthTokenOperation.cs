namespace Cql.Core.Owin.SwaggerDocs
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http.Description;

    using Swashbuckle.Swagger;

    public class AuthTokenOperation : IDocumentFilter
    {
        public bool SupportsClientId { get; set; } = true;

        public bool SupportsPasswordGrant { get; set; } = true;

        public bool SupportsRefreshTokenGrant { get; set; } = true;

        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            if (this.SupportsPasswordGrant)
            {
                this.AddTokenPasswordGrantEndPoint(swaggerDoc, schemaRegistry, apiExplorer);
            }

            if (this.SupportsRefreshTokenGrant)
            {
                this.AddRefreshTokenEndpoint(swaggerDoc, schemaRegistry, apiExplorer);
            }

            this.AddAdditionalAuthTokenEndPoints(swaggerDoc, schemaRegistry, apiExplorer);
        }

        protected virtual void AddAdditionalAuthTokenEndPoints(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
        }

        protected virtual void AddClientIdParams(ICollection<Parameter> list)
        {
            list.Add(
                new Parameter
                    {
                        type = "string",
                        name = "client_id",
                        @in = "formData",
                        required = false,
                        @default = "swagger",
                        description = "(Optional) pass client id in form data instead of using basic auth"
                    });
            list.Add(
                new Parameter
                    {
                        type = "password",
                        name = "client_secret",
                        @in = "formData",
                        required = false,
                        description = "(Optional) pass client secret in form data instead of using basic auth"
                    });
        }

        protected virtual void AddRefreshTokenEndpoint(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            swaggerDoc.paths.Add(
                "/api/token#refresh",
                new PathItem
                    {
                        post = new Operation
                                   {
                                       operationId = "RefreshTokenGrant",
                                       tags = new List<string> { "Token" },
                                       summary = "Used to retrieve an authorization token using a refresh token grant.",
                                       description = "The path should be /api/token -- it's a limitation of swagger that you cannot overload the same path and verb with different parameters.",
                                       consumes = FormEncodedContent(),
                                       responses = this.GetTokenReponse(),
                                       parameters = this.GetRefreshTokenGrantParameters()
                                   }
                    });
        }

        protected virtual void AddTokenPasswordGrantEndPoint(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            swaggerDoc.paths.Add(
                "/api/token",
                new PathItem
                    {
                        post = new Operation
                                   {
                                       operationId = "PasswordGrant",
                                       tags = new List<string> { "Token" },
                                       summary = "Used to retrieve an authorization token using a username and password grant.",
                                       consumes = FormEncodedContent(),
                                       responses = this.GetTokenReponse(),
                                       parameters = this.GetPasswordGrantParameters()
                                   }
                    });
        }

        protected virtual IDictionary<string, Schema> GetDefaultTokenResponseProperties()
        {
            var props = new Dictionary<string, Schema>();

            this.SetTokenProps(props);
            this.SetUserProps(props);
            this.SetAdditionalProps(props);

            return props;
        }

        protected virtual List<Parameter> GetPasswordGrantParameters()
        {
            var list = new List<Parameter>
                           {
                               new Parameter
                                   {
                                       type = "string",
                                       name = "grant_type",
                                       required = true,
                                       @in = "formData",
                                       @default = "password",
                                       description = "Always \"password\""
                                   },
                               new Parameter
                                   {
                                       type = "string",
                                       name = "username",
                                       required = true,
                                       @in = "formData",
                                       description = "The UserName (which is also their email)."
                                   },
                               new Parameter
                                   {
                                       type = "string",
                                       name = "password",
                                       required = true,
                                       @in = "formData",
                                       description = "The user's secret password.",
                                       format = "password"
                                   }
                           };

            if (this.SupportsClientId)
            {
                this.AddClientIdParams(list);
            }

            return list;
        }

        protected virtual List<Parameter> GetRefreshTokenGrantParameters()
        {
            var list = new List<Parameter>
                           {
                               new Parameter
                                   {
                                       type = "string",
                                       name = "grant_type",
                                       required = true,
                                       @in = "formData",
                                       @default = "refresh_token",
                                       description = "Always \"refresh_token\""
                                   },
                               new Parameter
                                   {
                                       type = "string",
                                       name = "refresh_token",
                                       required = true,
                                       @in = "formData",
                                       description = "The refresh token issued by the password grant step."
                                   }
                           };

            if (this.SupportsClientId)
            {
                this.AddClientIdParams(list);
            }

            return list;
        }

        protected virtual IDictionary<string, Response> GetTokenReponse()
        {
            return new Dictionary<string, Response>
                       {
                           ["200"] = new Response
                                         {
                                             description = "Login token response",
                                             schema = new Schema { properties = this.GetDefaultTokenResponseProperties() }
                                         }
                       };
        }

        protected virtual void SetAdditionalProps(IDictionary<string, Schema> props)
        {
        }

        protected virtual void SetTokenProps(IDictionary<string, Schema> props)
        {
            props["access_token"] = new Schema
                                        {
                                            example = "6coMuP3pLcjfQb1UOGIoMYBQ9wZ8_orMWYGwpPNQPzNH1ET8NyOLmzB2PpTI7SEI9fV7DWjOwQbOhvQDVAIfNgMMbrv8CrIOdWePd9Wkzuqhfqq8NIx=",
                                            type = "string",
                                            description = "The access token to be used in the Authentication header with a value of \"bearer {access_token}\" "
                                                          + "or access_token query parameter, \"?access_token={access_token}\" for authenticated links."
                                        };

            props["token_type"] = new Schema { example = "bearer", type = "string", description = "Always \"bearer\"." };

            props["expires_in"] = new Schema { example = 1799, type = "number", description = "The number of seconds before the issued access_token expires (about 30 minutes)." };

            props["refresh_token"] = new Schema
                                         {
                                             example = $"{Guid.NewGuid():n}{Guid.NewGuid():n}",
                                             type = "string",
                                             description = "The refresh token used for renewing the authenticated session when the current token expires."
                                         };
        }

        protected virtual void SetUserProps(IDictionary<string, Schema> props)
        {
            props["userName"] = new Schema { example = "user@cqlcorp.com", type = "string", description = "The username." };

            props["displayName"] = new Schema { example = "Ima User", type = "string", description = "The preferred display name." };

            props["roles"] = new Schema { example = "Admin", type = "string", description = "The list of roles for the user." };
        }

        private static List<string> FormEncodedContent()
        {
            return new List<string> { "application/x-www-form-urlencoded" };
        }
    }
}

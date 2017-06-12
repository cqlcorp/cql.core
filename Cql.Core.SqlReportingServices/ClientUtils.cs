using System;
using System.Net;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Cql.Core.SqlReportingServices
{
    public static class ClientUtils
    {
        public static Binding BasicAuthSecureTransportBinding()
        {
            return new BasicHttpsBinding
            {
                Security =
                {
                    Transport =
                    {
                        ClientCredentialType = HttpClientCredentialType.Basic
                    }
                },
                MaxReceivedMessageSize = int.MaxValue
            };
        }

        public static Binding BasicAuthTransportBinding()
        {
            return new BasicHttpBinding
            {
                Security =
                {
                    Mode = BasicHttpSecurityMode.TransportCredentialOnly,
                    Transport =
                    {
                        ClientCredentialType = HttpClientCredentialType.Basic
                    }
                },
                MaxReceivedMessageSize = int.MaxValue
            };
        }

        public static Binding NtlmSecureTransportBinding()
        {
            return new BasicHttpsBinding
            {
                Security =
                {
                    Transport =
                    {
                        ClientCredentialType = HttpClientCredentialType.Ntlm
                    }
                },
                MaxReceivedMessageSize = int.MaxValue
            };
        }

        public static Binding NtlmTransportBinding()
        {
            return new BasicHttpBinding
            {
                Security =
                {
                    Mode = BasicHttpSecurityMode.TransportCredentialOnly,
                    Transport =
                    {
                        ClientCredentialType = HttpClientCredentialType.Ntlm
                    }
                },
                MaxReceivedMessageSize = int.MaxValue
            };
        }

        public static void SetCredentials<TChannel>(
            this ClientBase<TChannel> client,
            NetworkCredential networkCredential = null) where TChannel : class
        {
            var clientCredentials = client.ClientCredentials;

            if (clientCredentials == null)
            {
                throw new InvalidOperationException("The client credentials may not be null");
            }

            var credentialType = client.GetCredentialType();

            switch (credentialType)
            {
                case HttpClientCredentialType.None:
                case HttpClientCredentialType.Digest:
                case HttpClientCredentialType.Certificate:
                case HttpClientCredentialType.InheritedFromHost:
                    break;

                case HttpClientCredentialType.Basic:

                    var userNamePassword = clientCredentials.UserName;

                    if (networkCredential != null)
                    {
                        userNamePassword.UserName = networkCredential.UserNameWithDomain();
                        userNamePassword.Password = networkCredential.Password;
                    }
                    break;

                case HttpClientCredentialType.Ntlm:
                case HttpClientCredentialType.Windows:

                    var windowsCredentials = clientCredentials.Windows;

                    if (networkCredential != null)
                    {
                        windowsCredentials.ClientCredential = networkCredential;
                    }
                    windowsCredentials.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static HttpClientCredentialType GetCredentialType<TChannel>(this ClientBase<TChannel> client)
            where TChannel : class
        {
            var credType =
                (client.Endpoint.Binding as BasicHttpBinding)?.Security?.Transport?.ClientCredentialType ??
                (client.Endpoint.Binding as BasicHttpsBinding)?.Security?.Transport?.ClientCredentialType;

            return credType ?? HttpClientCredentialType.Ntlm;
        }

        internal static string UserNameWithDomain(this NetworkCredential networkCredential)
        {
            return $@"{networkCredential.Domain}\{networkCredential.UserName}".Trim('\\');
        }
    }
}

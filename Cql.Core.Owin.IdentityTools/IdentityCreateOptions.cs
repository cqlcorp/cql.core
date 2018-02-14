using System;
using System.Collections.Generic;
using System.Threading;

namespace Cql.Core.Owin.IdentityTools
{
    public class IdentityCreateOptions
    {
        private List<KeyValuePair<string, string>> _additionalClaims;
        private List<string> _roles;

        public string AuthenticationType { get; set; }

        public string IdentityProviderName { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public List<string> Roles => LazyInitializer.EnsureInitialized(ref _roles, () => new List<string>());

        public List<KeyValuePair<string, string>> AdditionalClaims => LazyInitializer.EnsureInitialized(ref _additionalClaims, () => new List<KeyValuePair<string, string>>());
    }
}

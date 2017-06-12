using System;

namespace Cql.Core.Common.Types
{
    public class DirectoryUserInfo
    {
        public string DisplayName { get; set; }

        public Guid? Guid { get; set; }

        public string Email { get; set; }

        public string[] Roles { get; set; }
    }
}

using System;

namespace Cql.Core.Common.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public sealed class DataValueAttribute : Attribute
    {
        public DataValueAttribute(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }
    }
}

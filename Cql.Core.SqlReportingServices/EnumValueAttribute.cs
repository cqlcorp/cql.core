// ReSharper disable CheckNamespace

namespace Cql.Core.ReportingServices.ReportExecution
{
    using System;

    [AttributeUsage(AttributeTargets.All)]
    public sealed class EnumValueAttribute : Attribute
    {
        public EnumValueAttribute(string value)
        {
            this.Value = value;
        }

        public string Value { get; }
    }
}

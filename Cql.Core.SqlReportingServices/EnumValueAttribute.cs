// ReSharper disable CheckNamespace

using System;

namespace Cql.Core.ReportingServices.ReportExecution
{
    [AttributeUsage(AttributeTargets.All)]
    public sealed class EnumValueAttribute : Attribute
    {
        public EnumValueAttribute(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }
    }
}

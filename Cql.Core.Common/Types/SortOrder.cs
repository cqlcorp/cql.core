namespace Cql.Core.Common.Types
{
    using System.ComponentModel;

    using Cql.Core.Common.Attributes;

    public enum SortOrder
    {
        [Description("Ascending")]
        [DataValue("ASC")]
        Asc = 0,

        [Description("Descending")]
        [DataValue("DESC")]
        Desc = 1
    }
}

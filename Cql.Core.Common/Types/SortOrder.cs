using System.ComponentModel;
using Cql.Core.Common.Attributes;

namespace Cql.Core.Common.Types
{
    public enum SortOrder
    {
        [Description("Ascending")]
        [DataValue("ASC")]
        Asc = 0,

        [Description("Descending")]
        [DataValue("DESC")]
        Desc
    }
}

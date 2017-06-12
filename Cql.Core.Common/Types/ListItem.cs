using System;
using System.Diagnostics;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Cql.Core.Common.Types
{
    [DebuggerDisplay("{Text} = {Value}")]
    public class ListItem
    {
        public ListItem()
        {
        }

        public ListItem(string text, bool? selected = null) : this(text, text, selected)
        {
        }

        [StringFormatMethod("format")]
        public ListItem(string text, object value, string format = null, bool? selected = null)
        {
            Value = string.IsNullOrEmpty(format) ? Convert.ToString(value) : string.Format(format, value);
            Text = text;
            Selected = selected;
        }

        public ListItem(string text, string value, bool? selected = null)
        {
            Value = value;
            Text = text;
            Selected = selected;
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? Selected { get; set; }

        public string Text { get; set; }

        public string Value { get; set; }
    }
}

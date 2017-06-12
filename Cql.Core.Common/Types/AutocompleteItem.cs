using System;

namespace Cql.Core.Common.Types
{
    public class AutocompleteItem
    {
        public AutocompleteItem() : this("")
        {
        }

        public AutocompleteItem(object id, object text) : this (Convert.ToString(id), Convert.ToString(text))
        {
        }

        public AutocompleteItem(string text) : this(text, text)
        {
        }

        public AutocompleteItem(string id, string text)
        {
            Id = id;
            Text = text;
        }

        public string Id { get; set; }

        public string Text { get; set; }
    }
}

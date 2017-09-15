using System;

using Microsoft.AspNetCore.Mvc;

namespace Cql.Core.AspNetCore
{
    public static class ExtensionsForActionContext
    {
        public static string GetActionName(this ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!context.RouteData.Values.TryGetValue("action", out var obj))
            {
                return null;
            }

            var actionDescriptor = context.ActionDescriptor;

            string a = null;

            if (actionDescriptor.RouteValues.TryGetValue("action", out var str) && !string.IsNullOrEmpty(str))
            {
                a = str;
            }
            var b = obj?.ToString();

            return string.Equals(a, b, StringComparison.OrdinalIgnoreCase) ? a : b;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cql.Core.Common.Utils
{
    public class RuntimeHelper
    {
        public static bool TrueAtRuntime()
        {
            return 1 == int.Parse("1");
        }

        public static bool FalseAtRuntime()
        {
            return !TrueAtRuntime();
        }

        public static void Attempt(Action method)
        {
            try
            {
                method?.Invoke();
            }
            catch
            {
                // ignored
            }
        }

        public static bool IsDefaultValue<T>(T value)
        {
            return EqualityComparer<T>.Default.Equals(value, default(T));
        }

        public static async Task Attempt(Task method)
        {
            try
            {
                if (method != null)
                {
                    await method.ConfigureAwait(false);
                }
            }
            catch
            {
                // ignored
            }
        }
    }
}

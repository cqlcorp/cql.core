namespace Cql.Core.Caching
{
    using System;
    using System.Runtime.Caching;
    using System.Threading;

    public static class Cache
    {
        public static TimeSpan DefaultTimeToLive = TimeSpan.FromHours(1);

        private static MemoryCache _singletonInstance;

        public static MemoryCache MemoryCacheInstance
        {
            get
            {
                return LazyInitializer.EnsureInitialized(ref _singletonInstance, () => new MemoryCache("MemoryCacheService"));
            }
        }
    }
}

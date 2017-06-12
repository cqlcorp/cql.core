using System;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace Cql.Core.Caching
{
    public class MemoryCacheService : IMemoryCache
    {
        private static MemoryCache CacheInstance => Cache.MemoryCacheInstance;

        public Task<long> GetCountAsync() => Task.FromResult(CacheInstance.GetCount());

        public Task SetAsync(string key, object value, TimeSpan? timeToLive = null)
        {
            CacheInstance.Set(
                new CacheItem(key, value),
                new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now + timeToLive.GetValueOrDefault(Cache.DefaultTimeToLive) });

            return Task.CompletedTask;
        }

        public Task FlushAsync()
        {
            CacheInstance.Trim(100);

            return Task.CompletedTask;
        }

        public Task<T> GetAsync<T>(string key)
        {
            var cachedValue = Get<T>(key);

            return Task.FromResult(cachedValue.Value);
        }

        public Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> valueFactory)
        {
            return GetOrCreateAsync(key, null, valueFactory);
        }

        public async Task<T> GetOrCreateAsync<T>(string key, TimeSpan? timeToLive, Func<Task<T>> valueFactory)
        {
            var cachedValue = Get<T>(key);

            if (cachedValue.FoundInCache)
            {
                return cachedValue.Value;
            }

            var value = await valueFactory();

            await SetAsync(key, value, timeToLive);

            return value;
        }

        public Task RemoveAsync(string key)
        {
            CacheInstance.Remove(key);

            return Task.CompletedTask;
        }

        private static CachedValue<T> Get<T>(string key)
        {
            var value = CacheInstance.Get(key);

            return new CachedValue<T>(value);
        }

        private class CachedValue<TValue>
        {
            public CachedValue(object value)
            {
                FoundInCache = value != null;
                Value = ChangeType<TValue>(value);
            }

            public bool FoundInCache { get; }

            public TValue Value { get; }

            private static T ChangeType<T>(object value)
            {
                if (value == null)
                {
                    return default(T);
                }

                try
                {
                    return (T)value;
                }
                catch
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
            }
        }
    }
}

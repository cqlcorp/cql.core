namespace Cql.Core.Caching
{
    using System;
    using System.Threading.Tasks;

    public interface IMemoryCache
    {
        Task FlushAsync();

        Task<T> GetAsync<T>(string key);

        Task<long> GetCountAsync();

        Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> valueFactory);

        Task<T> GetOrCreateAsync<T>(string key, TimeSpan? timeToLive, Func<Task<T>> valueFactory);

        Task RemoveAsync(string key);

        Task SetAsync(string key, object value, TimeSpan? timeToLive = null);
    }
}

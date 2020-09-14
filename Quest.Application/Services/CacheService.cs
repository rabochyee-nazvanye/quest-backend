using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LazyCache;

namespace Quest.Application.Services
{
    public enum CacheName
    {
        ProgressBoardMain,
        ProgressBoardSingleEntry,
        ScoreBoard,
        ScoreBoardSingleEntry
    }

    public interface ICacheService
    {
        Task<T> GetOrAddAsync<T>(CacheName name, string key, Func<Task<T>> addItemFactory);
        void Invalidate(CacheName name, string key);
    }

    public class CacheService : ICacheService
    {
        private readonly IAppCache _cache;
        private const int DefaultCacheLifetimeInMinutes = 15;

        private readonly Dictionary<CacheName, int> _cacheLifetime = new Dictionary<CacheName, int>()
        {
            {CacheName.ProgressBoardMain, 15},
            {CacheName.ProgressBoardSingleEntry, 15},
            {CacheName.ScoreBoard, 15},
            {CacheName.ScoreBoardSingleEntry, 15},
        };

        public CacheService(IAppCache cache)
        {
            _cache = cache;
        }

        public async Task<T> GetOrAddAsync<T>(CacheName name, string key, Func<Task<T>> addItemFactory)
        {
            return await _cache.GetOrAddAsync(GetUnderlyingCacheKey(name, key), addItemFactory,
                GetExpirationOffset(name));
        }

        public void Invalidate(CacheName name, string key)
        {
            _cache.Remove(GetUnderlyingCacheKey(name, key));
        }

        private static string GetUnderlyingCacheKey(CacheName name, string key) => $"{name.ToString()}{key}";

        private DateTimeOffset GetExpirationOffset(CacheName name)
        {
            var lifeTime = DefaultCacheLifetimeInMinutes;
            if (_cacheLifetime.TryGetValue(name, out var time))
            {
                lifeTime = time;
            }

            return DateTimeOffset.Now.AddMinutes(lifeTime);
        }
    }
}
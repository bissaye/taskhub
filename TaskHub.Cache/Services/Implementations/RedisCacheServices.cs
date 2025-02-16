﻿using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using TaskHub.Cache.Services.Interfaces;

namespace TaskHub.Cache.Services.Implementations
{
    public class RedisCacheServices : ICacheServices
    {
        private readonly IDistributedCache _cache;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public RedisCacheServices(IDistributedCache cache, IConfiguration configuration, ILogger logger)
        {
            _cache = cache;
            _configuration = configuration;
            _logger = logger;
        }


        public async Task<T?> GetCachedDateAsync<T>(string cacheKey)
        {
            try
            {
                var cacheData = await _cache.GetStringAsync(cacheKey);
                if (cacheData == null) return default;

                return JsonConvert.DeserializeObject<T>(cacheData);

            }catch( Exception ex)
            {
                _logger.LogError("Cache Error: {Message}", ex.Message);
                return default;
            }

        }

        public  async Task RemovedCachedDataAsync(string cacheKey)
        {
            try
            {
                await _cache.RemoveAsync(cacheKey);
            }
            catch (Exception ex)
            {
                _logger.LogError("Cache Error: {Message}", ex.Message);
            }
        }

        public async Task SetCachedDateAsync<T>(string cacheKey, T data, TimeSpan timeToLive)
        {
            try
            {
                var serializedData = JsonConvert.SerializeObject(data);
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = timeToLive,
                };
                await _cache.SetStringAsync(cacheKey, serializedData, options);
            }
            catch (Exception ex)
            {
                _logger.LogError("Cache Error: {Message}", ex.Message);
            }
        }

        public async Task InvalidateDatasAsync(string pattern)
        {
            try
            {
                var server = ConnectionMultiplexer.Connect(_configuration.GetConnectionString("Redis")).GetServer(_configuration.GetConnectionString("Redis"));
                foreach (var cacheKey in server.Keys(pattern: pattern))
                {
                    await _cache.RemoveAsync(cacheKey);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Cache Error: {Message}", ex.Message);
            }

        }
    }
}

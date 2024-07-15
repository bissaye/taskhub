using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskHub.Cache.Services.Interfaces
{
    public interface ICacheServices
    {
        Task<T?> GetCachedDateAsync<T>(string cacheKey);
        Task SetCachedDateAsync<T>(string cacheKey, T data, TimeSpan timeToLive);
        Task RemovedCachedDataAsync(string cacheKey);
        Task InvalidateDatasAsync(string pattern);
    }
}

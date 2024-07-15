using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using TaskHub.Cache.Services.Implementations;
using TaskHub.Cache.Services.Interfaces;

namespace TaskHub.Cache
{
    public static class CacheServiceExtensions
    {
        public static void AddTaskHubCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.InstanceName = "TaskHub_Business_";
                options.ConfigurationOptions = new ConfigurationOptions
                {
                    EndPoints = { configuration.GetConnectionString("Redis") },
                    ConnectTimeout = 1000,
                    SyncTimeout = 1000,
                    AsyncTimeout = 1000
                };
            });

            services.AddSingleton<ICacheServices, RedisCacheServices>();
        }
    }
}
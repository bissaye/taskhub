using Microsoft.Extensions.DependencyInjection;
using TaskHub.Data;
using Microsoft.Extensions.Configuration;
using TaskHub.Business.Services.Implementations;
using TaskHub.Business.Services.Interfaces;
using TaskHub.Business.UseCases.Interfaces;
using TaskHub.Business.UseCases.Implementations;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace TaskHub.Business
{
    public static class BusinessServiceExtension
    {
        public static void AddTaskHubBusinessServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPasswordServices, PasswordServices>();

            services.AddScoped<IUserServices, UserServices>();

            services.AddScoped<ITaskItemsServices, TaskItemsServices>();

            services.AddScoped<ITokenServices, TokenServices>();

            services.AddSingleton<ICacheServices, RedisCacheServices>();

        }

        public static void AddTaskHubBusinessUsesCases(this IServiceCollection services)
        {
            services.AddScoped<IAuthUseCases, AuthUseCases>();

            services.AddScoped<IUserUsesCases, UserUsesCases>();

            services.AddScoped<ITaskItemUseCases, TaskItemUseCases>();


        }

        public static void AddTaskHubBusinessCache(this IServiceCollection services, IConfiguration configuration)
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
        }
        

    }
}

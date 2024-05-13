using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskHub.Data.Repositories;
using TaskHub.Data;
using TaskHub.Business.Services;
using Microsoft.Extensions.Configuration;

namespace TaskHub.Business
{
    public static class BusinessServiceExtension
    {
        public static void AddTaskHubBusinessServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<PasswordServices>(serviceProvider =>
            {
                return new PasswordServices();
            });

            services.AddScoped<UserServices>(serviceProvider =>
            {
                Gateway gateway = serviceProvider.GetRequiredService<Gateway>();
                return new UserServices(gateway);
            });

            services.AddScoped<TaskItemsServices>(serviceProvider =>
            {
                Gateway gateway = serviceProvider.GetRequiredService<Gateway>();
                return new TaskItemsServices(gateway);
            });

            services.AddScoped<UserServices>(serviceProvider =>
            {
                Gateway gateway = serviceProvider.GetRequiredService<Gateway>();
                return new UserServices(gateway);
            });

            services.AddScoped<TokenServices>(serviceProvider =>
            {
                Gateway gateway = serviceProvider.GetRequiredService<Gateway>();
                return new TokenServices(configuration);
            });

        }

    }
}

using Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.Configuration;
using TaskHub.Business.Services.Implementations;
using TaskHub.Business.Services.Interfaces;
using TaskHub.Business.UseCases.Interfaces;
using TaskHub.Business.UseCases.Implementations;

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

        }

        public static void AddTaskHubBusinessUsesCases(this IServiceCollection services)
        {
            services.AddScoped<IAuthUseCases, AuthUseCases>();

            services.AddScoped<IUserUsesCases, UserUsesCases>();

            services.AddScoped<ITaskItemUseCases, TaskItemUseCases>();


        }
        

    }
}

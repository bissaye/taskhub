using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskHub.Data.Repositories.Implementations;
using TaskHub.Data.Repositories.Interfaces;

namespace TaskHub.Data
{
    public static class DataServicesExtensions
    {
        public static void AddTaskHubDatabase(this IServiceCollection services, IConfiguration configuration, string assembly)
        {
            string connectionString = configuration.GetSection("SQLiteConnection").GetSection("SQLiteConnection").Value;

            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(connectionString, b => b.MigrationsAssembly(assembly)); 
            });

        }
        public static void AddTaskHubRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository>(serviceProvider =>
            {
                var dbContext = serviceProvider.GetService<DataContext>();
                return new UserRepository(dbContext);
            });

            services.AddScoped<ITaskItemRepository>(serviceProvider =>
            {
                var dbContext = serviceProvider.GetService<DataContext>();
                return new TaskItemRepository(dbContext);
            });
        }

        public static void AddTaskHubGateway(this IServiceCollection services)
        {
            services.AddScoped<IGateway>(serviceProvider =>
            {
                return new Gateway(serviceProvider);
            });
        }
    }

}

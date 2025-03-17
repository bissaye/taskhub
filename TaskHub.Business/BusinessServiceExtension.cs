using Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.Configuration;
using TaskHub.Business.Services.Implementations;
using TaskHub.Business.Services.Interfaces;
using TaskHub.Business.UseCases.Interfaces;
using TaskHub.Business.UseCases.Implementations;
using Bissaye.JwtAuth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace TaskHub.Business
{
    public static class BusinessServiceExtension
    {
        public static void AddTaskHubBusinessServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPasswordServices, PasswordServices>();

            services.AddScoped<IUserServices, UserServices>();

            services.AddScoped<ITaskItemsServices, TaskItemsServices>();


        }

        public static void AddTaskHubBusinessUsesCases(this IServiceCollection services)
        {
            services.AddScoped<IAuthUseCases, AuthUseCases>();

            services.AddScoped<IUserUsesCases, UserUsesCases>();

            services.AddScoped<ITaskItemUseCases, TaskItemUseCases>();


        }

        public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddBissayeJwtAuth(configuration,  configureJwtBearerEvents: options =>
            {
               options = new JwtBearerEvents {
                   OnChallenge = (context) =>
                   {
                       string result = "";
                       context.HandleResponse();
                       context.Response.StatusCode = 401;
                       context.Response.ContentType = "application/json";

                       if (!context.HttpContext.Request.Headers.ContainsKey("Authorization"))
                       {
                           result = JsonConvert.SerializeObject(new
                           {
                               errorNumber = 2,
                               value = "non authorisé",
                               message = "mauvais paramètres d'authorization"
                           });
                       }
                       else
                       {
                           result = JsonConvert.SerializeObject(new
                           {
                               errorNumber = 2,
                               value = "non authorisé",
                               detail = "mauvais paramètres d'authorization"
                           });
                       }
                       return context.Response.WriteAsync(result);
                   },

                   OnAuthenticationFailed = (context) => {
                       return context.Response.WriteAsync("test");
                   },
               };
            });
        }
        

    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaskHub.Business.Models.Custum;
using TaskHub.Business.Models.Errors;
using TaskHub.Data.Models.Errors;

namespace TaskHub.Business
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            HttpStatusCode statusCode;
            GenericResponse response;

            switch (ex)
            {
                case BadCredentialsErrorException badCredentialsError:
                    statusCode = HttpStatusCode.Unauthorized;
                    response = CustomHttpErrorNumber.badCredentials;
                    response.detail = badCredentialsError.Message;
                    _logger.LogWarning("Bad Credentials error: {Message}", badCredentialsError.Message);
                    break;
                case BadTokenErrorException badTokenError:
                    statusCode = HttpStatusCode.Unauthorized;
                    response = CustomHttpErrorNumber.badCredentials;
                    response.detail = badTokenError.Message;
                    _logger.LogWarning("Bad token error: {Message}", badTokenError.Message);
                    break;
                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    response = CustomHttpErrorNumber.notfound;
                    response.detail = notFoundException.Message;
                    _logger.LogWarning("Resource not found: {Message}", notFoundException.Message);
                    break;
                case UnauthorizedAccessException unauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;
                    response = CustomHttpErrorNumber.unauthorized;
                    response.detail = unauthorizedAccessException.Message;
                    _logger.LogWarning("Unauthorized access to read data: {Message}", unauthorizedAccessException.Message);
                    break;
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    response = CustomHttpErrorNumber.serverError;
                    response.detail = ex.Message;
                    _logger.LogError("Internal server error: {Message}", ex.Message);
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var result = JsonConvert.SerializeObject(response);

            return context.Response.WriteAsync(result);
        }
    }
    
}

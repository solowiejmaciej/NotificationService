using System.Threading.Tasks;
using AuthService.Application.Models.AppSettings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace AuthService.Api.Middleware
{
    public class ApiKeyAuthMiddleware : IMiddleware
    {
        private readonly IOptions<ApiKeySettings> _config;

        public ApiKeyAuthMiddleware(
            IOptions<ApiKeySettings> config
        )
        {
            _config = config;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (!context.Request.Headers.TryGetValue(_config.Value.HeaderName, out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key missing");
                return;
            }

            var apiKey = _config.Value.ApiKey;
            if (!apiKey.Equals(extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid ApiKey");
                return;
            }

            await next(context);
        }
    }
}
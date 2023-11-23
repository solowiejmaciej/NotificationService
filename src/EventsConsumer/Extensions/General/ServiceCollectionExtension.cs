#region

using Shared.Middleware;

#endregion

namespace EventsConsumer.Extensions.General;

public static class ServiceCollectionExtension
{
    public static void AddGeneralServiceCollection(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("apiCorsPolicy",
                builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        //                .AllowCredentials()
                        .SetIsOriginAllowed(options => true);
                    //.WithMethods("OPTIONS", "GET");
                });
        });


        //HealthChecks
        services.AddHealthChecks();
        services.AddLogging();
        services.AddHttpContextAccessor();
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddScoped<ErrorHandlingMiddleware>();

        services.AddScoped<INotificationApiClient, NotificationApiClient>();
    }
}
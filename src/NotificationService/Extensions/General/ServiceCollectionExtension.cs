﻿#region

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using NotificationService.Entities;
using NotificationService.Health;
using Shared.Middleware;

#endregion

namespace NotificationService.Extensions.General;

public static class ServiceCollectionExtension
{
    public static void AddGeneralServiceCollection(this IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

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

        //Db
        services.AddDbContext<NotificationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("App"));
        });

        //Cache
        services.AddStackExchangeRedisCache(redisOptions =>
        {
            var connection = configuration.GetConnectionString("Redis");
            redisOptions.Configuration = connection;
        });

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));


        services.AddMemoryCache();

        //HealthChecks
        services.AddHealthChecks()
            .AddCheck<DatabaseHealthCheck>("mssqlDb")
            .AddCheck<CacheDbHealthCheck>("cache")
            .AddCheck<SmsPlanetApiHealthCheck>("smsPlanetApi");
        services.AddLogging();
        services.AddHttpContextAccessor();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddScoped<ErrorHandlingMiddleware>();
    }

    public static IApplicationBuilder AddPrometheus(this IApplicationBuilder app)
    {
        return app;
    }
}
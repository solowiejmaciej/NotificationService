using System.Reflection;
using BarcodeService.Application.Hangfire;
using BarcodeService.Domain.Interfaces;
using BarcodeService.Infrastructure.Hangfire.Manager;
using BarcodeService.Infrastructure.Persistent;
using BarcodeService.Infrastructure.Repositories;
using Hangfire;
using HangfireBasicAuthenticationFilter;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BarcodeService.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<IProductsRepository, ProductsRepository>();
        services.AddScoped<IPotentialProductsRepository, PotentialProductsRepository>();
        services.AddScoped<IProductJobManager, ProductJobManager>();

        services.AddHangfire(config => config
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(configuration.GetConnectionString("Hangfire")));

        services.AddHangfireServer((serviceProvider, bjsOptions) =>
        {
            bjsOptions.ServerName = "BarcodeServiceServer";
            bjsOptions.Queues = new[]
            {
                HangfireQueues.HIGH_PRIORITY,
                HangfireQueues.MEDIUM_PRIORITY,
                HangfireQueues.LOW_PRIORITY,
                HangfireQueues.DEFAULT
            };
        });

        //Db
        services.AddDbContext<BarcodesDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("App"));
        });

        //Cache
        services.AddStackExchangeRedisCache(redisOptions =>
        {
            redisOptions.Configuration = configuration.GetConnectionString("Redis");
        });
    }

    public static IApplicationBuilder UseHangfire(this IApplicationBuilder app, IConfiguration configuration)
    {
        var hangfireSettings = configuration.GetSection("HangfireSettings");
        
        app.UseHangfireDashboard("/hangfire", new DashboardOptions()
        {
            DashboardTitle = "NotificationService",
            Authorization = new[]
            {
                new HangfireCustomBasicAuthenticationFilter()
                {
                    User = hangfireSettings["UserName"],
                    Pass = hangfireSettings["Password"]
                }
            }
        });
        
        RecurringJob.AddOrUpdate<IPotentialProductsEvaluationService>(
            "Price evaluation job",
            x => x.Eval(),
            Cron.Hourly,
            queue: HangfireQueues.LOW_PRIORITY
        );
        
        return app;
        
    }
}
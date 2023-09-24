using AuthService.Application.Models.AppSettings;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Persistent;
using AuthService.Infrastructure.Repositories;
using AuthService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;

namespace AuthService.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //RabbitMq
        var rabbitConfig = new RabbitSettings();
        var rabbitConfigurationSection = configuration.GetSection("RabbitSettings");
        rabbitConfigurationSection.Bind(rabbitConfig);
        
        services.AddMassTransit(mt => mt.AddMassTransit(x => {
            x.UsingRabbitMq((context, cfg) => {
                cfg.Host(rabbitConfig.Url, "/", c => {
                    c.Username(rabbitConfig.Username);
                    c.Password(rabbitConfig.Password);
                });
                cfg.ConfigureEndpoints(context);
            });
        }));
        
        //Db
        services.AddDbContext<AuthServiceDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("App"));
        });
        
        //Cache
        services.AddStackExchangeRedisCache(redisOptions =>
        {
            redisOptions.Configuration = configuration.GetConnectionString("Redis");
        });

        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<IEventPublisher, EventPublisher>();
        
        services.AddScoped<IUsersRepository,UsersRepository>();
        services.Decorate<IUsersRepository, CachedUsersRepository>();

        services.AddScoped<IRefreshTokensRepository,RefreshTokensRepository>();
        services.AddScoped<IConfirmationCodesRepository,ConfirmationCodesRepository>();
    }
    
}
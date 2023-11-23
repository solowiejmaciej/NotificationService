#region

using EventsConsumer.Persistant;
using EventsConsumer.Repositories;
using Microsoft.EntityFrameworkCore;

#endregion

namespace EventsConsumer.Extensions.Db;

public static class ServiceCollectionExtensions
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEventsRepository, EventsRepository>();

        services.AddDbContext<EventsDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("App"));
        });
    }
}
#region

using EventsConsumer.Consumers;
using EventsConsumer.Events;
using EventsConsumer.Managers;
using EventsConsumer.Models.AppSettings;
using MassTransit;
using Shared.Events;

#endregion

namespace EventsConsumer.Extensions.Events;

public static class ServiceCollectionExtensions
{
    public static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitConfig = new RabbitSettings();
        var rabbitConfigurationSection = configuration.GetSection("RabbitSettings");
        rabbitConfigurationSection.Bind(rabbitConfig);


        //services.AddSingleton<IPublishEndpoint>(azureServiceBus);

        services.AddScoped<IEventManager<NotificationEvent>, NotificationsEventManager>();


        services.AddMassTransit(mt => mt.AddMassTransit(x =>
        {
            x.AddConsumer<UserCreatedConsumer>(); // Register the consumer
            x.AddConsumer<SendConfirmationCodeEventConsumer>(); // Register the consumer
            x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(rabbitConfig.Url, "/", c =>
                {
                    c.Username(rabbitConfig.Username);
                    c.Password(rabbitConfig.Password);
                });
                cfg.ReceiveEndpoint(RabbitQueues.NOTIFICATIONS_QUEUE, endpoint =>
                {
                    endpoint.ConfigureConsumer<UserCreatedConsumer>(context);
                    endpoint.ConfigureConsumer<SendConfirmationCodeEventConsumer>(context);
                });
            }));
        }));
    }
}
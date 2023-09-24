using EventsConsumer.Consumers;
using EventsConsumer.Models.AppSettings;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Events;

namespace EventsConsumer.Extensions.Events
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRabbitMq(this IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var rabbitConfig = new RabbitSettings();
            var rabbitConfigurationSection = configuration.GetSection("RabbitSettings");
            rabbitConfigurationSection.Bind(rabbitConfig);


            //services.AddSingleton<IPublishEndpoint>(azureServiceBus);




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
}

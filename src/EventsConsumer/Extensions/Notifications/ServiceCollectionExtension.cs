#region

using EventsConsumer.Models.AppSettings;

#endregion

namespace EventsConsumer.Extensions.Notifications;

public static class ServiceCollectionExtension
{
    public static void AddNotificationsServiceCollection(this IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var notificationApiConfig = configuration.GetSection("NotificationApiCreds");
        services.Configure<NotificationApiSettings>(notificationApiConfig);


        services.AddSingleton<INotificationApiClient, NotificationApiClient>();
    }
}
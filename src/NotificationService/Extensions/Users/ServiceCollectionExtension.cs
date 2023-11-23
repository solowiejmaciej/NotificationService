#region

using NotificationService.Models.AppSettings;
using NotificationService.Services;
using Shared.UserContext;

#endregion

namespace NotificationService.Extensions.Users;

public static class ServiceCollectionExtension
{
    public static void AddUsersServiceCollection(this IServiceCollection services)
    {
        //My services
        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<IUserService, UserService>();

        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var authApiConfig = configuration.GetSection("AuthApiSettings");
        services.Configure<AuthApiConfig>(authApiConfig);
    }
}
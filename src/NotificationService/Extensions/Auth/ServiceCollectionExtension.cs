#region

using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using NotificationService.Middleware;
using NotificationService.Models.AppSettings;

#endregion

namespace NotificationService.Extensions.Auth;

public static class ServiceCollectionExtension
{
    public static void AddAuthServiceCollection(this IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var jwtSettings = new JWTSettings();
        var authConfigurationSection = configuration.GetSection("AuthSettings");
        authConfigurationSection.Bind(jwtSettings);

        var apiKeySettings = new ApiKeySettings();
        var apiKeyConfigurationSection = configuration.GetSection("ApiKeySettings");
        apiKeyConfigurationSection.Bind(apiKeySettings);

        var rsa = RSA.Create();

        rsa.ImportSubjectPublicKeyInfo(
            Convert.FromBase64String(jwtSettings.PublicKey),
            out var _
        );

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = new RsaSecurityKey(rsa),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        services.AddSingleton(tokenValidationParameters);

        services.Configure<ApiKeySettings>(apiKeyConfigurationSection);
        services.Configure<JWTSettings>(authConfigurationSection);

        services.AddScoped<ApiKeyAuthMiddleware>();

        services.AddAuthentication(option =>
        {
            option.DefaultAuthenticateScheme = "Bearer";
            option.DefaultScheme = "Bearer";
            option.DefaultChallengeScheme = "Bearer";
        }).AddJwtBearer(cfg =>
        {
            cfg.RequireHttpsMetadata = false;
            cfg.SaveToken = true;
            cfg.TokenValidationParameters = tokenValidationParameters;
        });
    }
}
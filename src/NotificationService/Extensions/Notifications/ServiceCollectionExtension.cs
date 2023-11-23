﻿#region

using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using NotificationService.MappingProfiles.Notifications;
using NotificationService.MappingProfiles.Recipients;
using NotificationService.Models.AppSettings;
using NotificationService.Models.QueryParameters.Create;
using NotificationService.Models.Requests;
using NotificationService.Models.Requests.Add;
using NotificationService.Models.Validation.QueryParametersValidation;
using NotificationService.Models.Validation.RequestValidation;
using NotificationService.Repositories;
using NotificationService.Repositories.Cached;
using NotificationService.Services;

#endregion

namespace NotificationService.Extensions.Notifications;

public static class ServiceCollectionExtension
{
    public static void AddNotificationsServiceCollection(this IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var redisSettings = configuration.GetSection("RedisSettings");
        var smtpSettings = configuration.GetSection("SMTPSettings");
        var googleFirebaseSettings = configuration.GetSection("GoogleFirebaseSettings");
        var smsSettings = configuration.GetSection("SmsSettings");

        // Add services to the container.
        //Cache
        services.AddScoped<ICacheService, CacheService>();

        //Repos
        services.AddScoped<IEmailsRepository, EmailsRepository>();
        services.Decorate<IEmailsRepository, CachedEmailsRepository>();

        services.AddScoped<IPushRepository, PushRepository>();
        services.Decorate<IPushRepository, CachedPushRepository>();

        services.AddScoped<ISmsRepository, SmsRepository>();
        services.Decorate<ISmsRepository, CachedSmsRepository>();

        //Services
        services.AddScoped<IRecipientService, RecipientService>();

        //Config
        services.Configure<SMTPSettings>(smtpSettings);
        services.Configure<RedisSettings>(redisSettings);
        services.Configure<GoogleFirebaseSettings>(googleFirebaseSettings);
        services.Configure<SmsSettings>(smsSettings);

        //Validation
        services.AddFluentValidationAutoValidation();
        services.AddScoped<IValidator<AddEmailRequest>, AddEmailRequestValidation>();
        services.AddScoped<IValidator<AddSmsRequest>, AddSmsRequestValidation>();
        services.AddScoped<IValidator<AddPushRequest>, AddPushRequestValidation>();

        services.AddScoped<IValidator<CreateEmailRequestQueryParameters>, EmailRequestQuerryParametersValidation>();
        services.AddScoped<IValidator<CreateSmsRequestQueryParameters>, SmsRequestQuerryParametersValidation>();
        services.AddScoped<IValidator<PushRequestQuerryParameters>, PushRequestQuerryParametersValidation>();

        //Mapper
        services.AddScoped(provider => new MapperConfiguration(cfg =>
            {
                //var scope = provider.CreateScope();
                //var userContext = scope.ServiceProvider.GetRequiredService<IUserContext>();
                cfg.AddProfile(new EmailMappingProfile());
                cfg.AddProfile(new PushMappingProfile());
                cfg.AddProfile(new SmsMappingProfile());
                cfg.AddProfile(new RecipientMappingProfile());
            }).CreateMapper()
        );
    }
}
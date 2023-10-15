using System.Reflection;
using AutoMapper;
using BarcodeService.Application.ApplicationUserContext;
using BarcodeService.Application.Mappings;
using BarcodeService.Application.Models.AppSettings;
using BarcodeService.Application.Services;
using BarcodeService.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BarcodeService.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        var openFoodApiSettings = configuration.GetSection("OpenFoodApiSettings");
        services.Configure<OpenFoodApiSettings>(openFoodApiSettings);

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        //Mapper
        services.AddScoped(provider => new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new ProductsMappingProfile());
        }).CreateMapper());
        
        services.AddScoped<IOpenFoodApiClient, OpenFoodApiClient>();
        services.AddScoped<IPotentialProductsEvaluationService, PotentialProductsEvaluationService>();
        //services.AddScoped<IUserContext, UserContext>();

    }
    
}
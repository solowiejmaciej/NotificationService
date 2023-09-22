using System.Reflection;
using AuthService.Application.ApplicationUserContext;
using AuthService.Application.Mappings;
using AuthService.Application.Services;
using AuthService.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));


        //Mapper
        services.AddScoped(provider => new MapperConfiguration(cfg =>
        {
            //var scope = provider.CreateScope();
            //var userContext = scope.ServiceProvider.GetRequiredService<IUserContext>();

            cfg.AddProfile(new UserMappingProfile());
        }).CreateMapper());
        
        services.AddScoped<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();
        services.AddScoped<IJwtManager, JwtManager>();
        services.AddScoped<IUserContext, UserContext>();

    }
    
}
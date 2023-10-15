using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using BarcodeService.Api.Models.Requests;
using BarcodeService.Api.Models.Validation;
using BarcodeService.Application.Models.AppSettings;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace BarcodeService.Api.Extensions;


public static class ServiceCollectionExtension
{
    
    public static void AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFluentValidationAutoValidation();
        services.AddScoped<IValidator<AddNewPotentialProductRequest>, AddNewPotentialProductRequestValidation>();
        
        var jwtSettings = new JwtSettings();
        var authConfigurationSection = configuration.GetSection("AuthSettings");
        authConfigurationSection.Bind(jwtSettings);
        
        RSA rsa = RSA.Create();
        
        rsa.ImportSubjectPublicKeyInfo(
            source: Convert.FromBase64String(jwtSettings.PublicKey),
            bytesRead: out int _
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
        
        services.AddSwaggerGen(c =>
               {
                   c.SwaggerDoc("v1", new OpenApiInfo { Title = "BarcodeService", Version = "v1" });
               
                   var securityScheme = new OpenApiSecurityScheme
                   {
                       Name = "JWT Authentication",
                       Description = "Enter JWT Bearer token **_only_**",
                       In = ParameterLocation.Header,
                       Type = SecuritySchemeType.Http,
                       Scheme = "bearer",
                       BearerFormat = "JWT",
                       Reference = new OpenApiReference
                       {
                           Id = JwtBearerDefaults.AuthenticationScheme,
                           Type = ReferenceType.SecurityScheme
                       },
                   };
               
                  
                   c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
               
                   c.AddSecurityRequirement(new OpenApiSecurityRequirement
                   {
                       { securityScheme, new string[] { } }
                   });
               });
        
    }
}
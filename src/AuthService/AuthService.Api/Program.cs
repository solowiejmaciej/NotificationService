using AuthService.Api.Extensions;
using AuthService.Api.Middleware;
using AuthService.Api.Models.Requests.Add;
using AuthService.Api.Models.Requests.Login;
using AuthService.Api.Models.Validation.RequestValidation;
using AuthService.Application.Extensions;
using AuthService.Application.Health;
using AuthService.Infrastructure.Extensions;
using AuthService.Models.Requests.Update;
using FluentValidation;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();


builder.Services.AddSwaggerServiceCollection();
builder.Services.AddAuthServiceCollection();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<IValidator<UserLoginRequest>, UserLoginRequestValidation>();
builder.Services.AddScoped<IValidator<AddUserRequest>, AddUserRequestValidation>();
builder.Services.AddScoped<IValidator<UpdateUserRequest>, UpdateUserRequestValidation>();

builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<ApiKeyAuthMiddleware>();


builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("mssqlDb")
    .AddCheck<CacheDbHealthCheck>("cache");
var app = builder.Build();


app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseAuthorization();



app.MapHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapControllers();
//app.UseMiddleware<ApiKeyAuthMiddleware>();


app.Run();
using EventsConsumer.Extensions.Events;
using EventsConsumer.Extensions.General;
using EventsConsumer.Extensions.Notifications;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddRabbitMq();
builder.Services.AddGeneralServiceCollection();
builder.Services.AddNotificationsServiceCollection();

var app = builder.Build();

app.UseHttpsRedirection();


app.MapHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
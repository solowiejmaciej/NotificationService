using EventsConsumer.Extensions.Db;
using EventsConsumer.Extensions.Events;
using EventsConsumer.Extensions.General;
using EventsConsumer.Extensions.Notifications;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDatabase(configuration);
builder.Services.AddRabbitMq(configuration);
builder.Services.AddGeneralServiceCollection();
builder.Services.AddNotificationsServiceCollection();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();


app.UseHttpsRedirection();


app.MapHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
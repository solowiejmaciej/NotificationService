using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Events;

namespace EventsConsumer;

public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
{
    private readonly ILogger<UserCreatedConsumer> _logger;
    private readonly INotificationApiClient _client;

    public UserCreatedConsumer(
        ILogger<UserCreatedConsumer> logger,
        INotificationApiClient client
    )
    {
        _logger = logger;
        _client = client;
    }


    public Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        var userCreated = context.Message;
        _logger.LogInformation("Trying to send welcome email for user {UserId} ", userCreated.UserId);
        try
        {
            _client.SendEmail(userCreated.UserId);
        }
        catch (Exception badRequestException)
        {
            _logger.LogError(badRequestException.Message);
        }

        return Task.CompletedTask;
    }
}
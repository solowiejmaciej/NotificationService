using EventsConsumer.Models.Body;
using MassTransit;
using Shared.Events;

namespace EventsConsumer.Consumers;

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
        var body = new SendEmailBody($"Cześć dziękujemy za rejestrację!", "Hello there!");
        try
        {
            _client.SendEmail(userCreated.UserId, body);
        }
        catch (Exception badRequestException)
        {
            _logger.LogError(badRequestException.Message);
        }

        return Task.CompletedTask;
    }
}
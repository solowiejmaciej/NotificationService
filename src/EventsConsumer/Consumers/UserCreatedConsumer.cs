using EventsConsumer.Events;
using EventsConsumer.Managers;
using EventsConsumer.Models.Body;
using MassTransit;
using Shared.Enums;
using Shared.Events;

namespace EventsConsumer.Consumers;

public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
{
    private readonly ILogger<UserCreatedConsumer> _logger;
    private readonly INotificationApiClient _client;
    private readonly IEventManager<NotificationEvent> _eventManager;

    public UserCreatedConsumer(
        ILogger<UserCreatedConsumer> logger,
        INotificationApiClient client,
        IEventManager<NotificationEvent> eventManager
            )
    {
        _logger = logger;
        _client = client;
        _eventManager = eventManager;
    }


    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        var userCreated = context.Message;
        await _eventManager.AddAsync(userCreated);
        await _eventManager.ChangeStatusAsync(userCreated, EStatus.Processing);
        _logger.LogInformation("Trying to send welcome email for user {UserId} ", userCreated.UserId);
        var body = new SendEmailBody($"Cześć dziękujemy za rejestrację!", "Hello there!");
        try
        {
            _client.SendEmail(userCreated.UserId, body);
            await _eventManager.ChangeStatusAsync(userCreated, EStatus.Completed);
        }
        catch (Exception badRequestException)
        {
            _logger.LogError(badRequestException.Message);
            await _eventManager.ChangeStatusAsync(userCreated, EStatus.Failed);
            await _eventManager.AddErrorMessageAsync(userCreated, badRequestException.Message);
        }

    }
}
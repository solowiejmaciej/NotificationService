#region

using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using EventsConsumer.Events;
using MassTransit;

#endregion

namespace AuthService.Infrastructure.Services;

public class EventPublisher : IEventPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public EventPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishUserCreatedEventAsync(ApplicationUser user, CancellationToken cancellationToken = default)
    {
        await _publishEndpoint.Publish(new UserCreatedEvent
        {
            Firstname = user.Firstname,
            Surname = user.Surname,
            UserId = user.Id
        }, cancellationToken);
    }

    public async Task PublishSendConfirmationCodeEventAsync(ConfirmationCode confirmationCode, string userId,
        CancellationToken cancellationToken = default)
    {
        await _publishEndpoint.Publish(new SendConfirmationCodeEvent
        {
            Code = confirmationCode.Code,
            UserId = userId,
            NotificationChannel = confirmationCode.NotificationChannel
        }, cancellationToken);
    }
}
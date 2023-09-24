using AuthService.Domain.Entities;

namespace AuthService.Domain.Interfaces;

public interface IEventPublisher
{
    Task PublishUserCreatedEventAsync(ApplicationUser userDto, CancellationToken cancellationToken = default);

    Task PublishSendConfirmationCodeEventAsync(ConfirmationCode confirmationCode, string userId,
        CancellationToken cancellationToken = default);

}
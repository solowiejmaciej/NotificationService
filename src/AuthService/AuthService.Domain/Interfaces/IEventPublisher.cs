using AuthService.Domain.Entities;

namespace AuthService.Domain.Interfaces;

public interface IEventPublisher
{
    Task PublishUserCreatedEventAsync(ApplicationUser userDto, CancellationToken cancellationToken = default);
}
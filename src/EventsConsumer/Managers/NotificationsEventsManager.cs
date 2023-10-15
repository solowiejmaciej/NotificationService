using EventsConsumer.Abstractions;
using EventsConsumer.Events;
using EventsConsumer.Repositories;
using Shared.Enums;

namespace EventsConsumer.Managers;
public class NotificationsEventManager : IEventManager<NotificationEvent>
{
    private readonly IEventsRepository _eventsRepository;

    public NotificationsEventManager(
        IEventsRepository eventsRepository
        )
    {
        _eventsRepository = eventsRepository;
    }
    public async Task<bool> ChangeStatusAsync(NotificationEvent @event, EStatus status)
    {
        var baseEvent = @event.ToEvent();
        baseEvent.Status = status;
        await _eventsRepository.ChangeStatusAsync(baseEvent);
        return true;
    }

    public Task<bool> AddErrorMessageAsync(NotificationEvent @event, string errorMessage)
    {
        var baseEvent = @event.ToEvent();
        baseEvent.ErrorMessage = errorMessage;
        return _eventsRepository.UpdateAsync(baseEvent);
    }

    public async Task<bool> AddAsync(NotificationEvent @event)
    {
        var baseEvent = @event.ToEvent();
        await _eventsRepository.AddAsync(baseEvent);
        return true;
    }
}

#region

using EventsConsumer.Events;
using Shared.Enums;

#endregion

namespace EventsConsumer.Managers;

public interface IEventManager<in T> where T : NotificationEvent
{
    public Task<bool> ChangeStatusAsync(T @event, EStatus status);
    public Task<bool> AddErrorMessageAsync(T @event, string errorMessage);
    Task<bool> AddAsync(T @event);
}
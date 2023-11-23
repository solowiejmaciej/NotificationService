#region

using EventsConsumer.Abstractions;
using EventsConsumer.Models.Entity;
using Shared.Enums;
using Shared.Events;

#endregion

namespace EventsConsumer.Events;

public abstract class NotificationEvent : BaseEvent
{
    public string Firstname { get; set; }
    public string Surname { get; set; }
    public string UserId { get; set; }

    public new readonly string QueueName = RabbitQueues.NOTIFICATIONS_QUEUE;

    public override Event ToEvent()
    {
        return new Event
        {
            Id = Id,
            CreatedAt = DateTime.Now,
            QueueName = QueueName,
            Status = EStatus.Published,
            ErrorMessage = string.Empty,
            InternalName = GetType().Name
        };
    }
}
using EventsConsumer.Models.Entity;

namespace EventsConsumer.Abstractions;

public abstract class BaseEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string QueueName { get; set; }
    public virtual Event ToEvent()
    {
        throw new NotImplementedException();
    }
    
}
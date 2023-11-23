#region

using Shared.Enums;

#endregion

namespace EventsConsumer.Models.Entity;

public class Event
{
    public required Guid Id { get; set; }
    public required string InternalName { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required string QueueName { get; set; }
    public required EStatus Status { get; set; }
    public required string ErrorMessage { get; set; }
}
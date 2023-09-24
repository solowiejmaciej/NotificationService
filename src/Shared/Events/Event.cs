using Shared.Enums;

namespace Shared.Events;

public class Event
{
    Guid Id { get; set; }
    DateTime CreatedAt { get; set; }
    string QueueName { get; set; }
    EStatus Status { get; set; }
    string ErrorMessage { get; set; }
}

using Shared.Enums;

namespace Shared.Events;

public class SendConfirmationCodeEvent : Event
{
    public int Code { get; set; }
    public string UserId { get; set; }
    public ENotificationChannel NotificationChannel { get; set; }
    public DateTimeOffset ExpirationDate { get; set; }
    
}
#region

using Shared.Enums;

#endregion

namespace EventsConsumer.Events;

public class SendConfirmationCodeEvent : NotificationEvent
{
    public int Code { get; set; }
    public ENotificationChannel NotificationChannel { get; set; }
    public DateTimeOffset ExpirationDate { get; set; }
}
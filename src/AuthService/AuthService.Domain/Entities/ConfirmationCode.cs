#region

using Shared.Enums;

#endregion

namespace AuthService.Domain.Entities;

public record ConfirmationCode
{
    public int Code { get; set; } = new Random().Next(100000, 999999);
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTimeOffset ExpiresAt { get; set; } = DateTimeOffset.Now.AddMinutes(10);
    public ENotificationChannel NotificationChannel { get; set; }
}
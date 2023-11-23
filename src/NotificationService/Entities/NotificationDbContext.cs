#region

using Microsoft.EntityFrameworkCore;
using NotificationService.Entities.NotificationEntities;

#endregion

namespace NotificationService.Entities;

public class NotificationDbContext : DbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options)
    {
    }

    public DbSet<EmailNotification> EmailsNotifications { get; set; }
    public DbSet<PushNotification> PushNotifications { get; set; }
    public DbSet<SmsNotification> SmsNotifications { get; set; }
}
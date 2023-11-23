#region

using Microsoft.Extensions.Diagnostics.HealthChecks;
using NotificationService.Entities;

#endregion

namespace NotificationService.Health;

public class DatabaseHealthCheck : IHealthCheck
{
    private readonly NotificationDbContext _context;

    public DatabaseHealthCheck(NotificationDbContext context)
    {
        _context = context;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = new())
    {
        var canConnectAsyncResult = await _context.Database.CanConnectAsync(cancellationToken);
        if (!canConnectAsyncResult) return HealthCheckResult.Unhealthy();
        return HealthCheckResult.Healthy();
    }
}
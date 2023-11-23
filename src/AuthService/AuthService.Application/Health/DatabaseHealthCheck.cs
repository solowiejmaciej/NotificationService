#region

using AuthService.Domain.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;

#endregion

namespace AuthService.Application.Health;

public class DatabaseHealthCheck : IHealthCheck
{
    private readonly IUsersRepository _repository;

    public DatabaseHealthCheck(IUsersRepository repository)
    {
        _repository = repository;
    }


    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = new())
    {
        var canConnectAsyncResult = await _repository.CanConnectAsync(cancellationToken);
        if (!canConnectAsyncResult) return HealthCheckResult.Unhealthy();
        return HealthCheckResult.Healthy();
    }
}
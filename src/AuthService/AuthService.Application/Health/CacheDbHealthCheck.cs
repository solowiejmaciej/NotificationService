using System;
using System.Threading;
using System.Threading.Tasks;
using AuthService.Domain.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AuthService.Application.Health
{
    public class CacheDbHealthCheck : IHealthCheck
    {
        private readonly ICacheService _cacheService;

        public CacheDbHealthCheck(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                await _cacheService.SetDataAsync("Health", "I'm healthy!", DateTimeOffset.Now.AddMinutes(2), cancellationToken);
                await _cacheService.GetDataAsync<string>("Health", cancellationToken);
            }
            catch (Exception e)
            {
                return HealthCheckResult.Degraded(e.Message);
            }

            return HealthCheckResult.Healthy();
        }
    }
}
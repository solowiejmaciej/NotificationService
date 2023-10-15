#region

using AuthService.Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

#endregion

namespace AuthService.Infrastructure.Services;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;

    public CacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<T?> GetDataAsync<T>(string key,
        CancellationToken cancellationToken = default)
    {
        var value = await _distributedCache.GetStringAsync(key, cancellationToken);
        if (!string.IsNullOrEmpty(value)) return JsonSerializer.Deserialize<T>(value);
        return default;
    }

    public async Task SetDataAsync<T>(string key, T value, DateTimeOffset expirationTime,
        CancellationToken cancellationToken = default)
    {
        var expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
        await _distributedCache.SetStringAsync(
            key,
            JsonConvert.SerializeObject(value),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiryTime
            },
            cancellationToken
        );
    }

    public async Task RemoveDataAsync(string key, CancellationToken cancellationToken = default)
    {
        await _distributedCache.RemoveAsync(key, cancellationToken);
    }
}
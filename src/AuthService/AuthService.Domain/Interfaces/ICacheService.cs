namespace AuthService.Domain.Interfaces;

public interface ICacheService
{
    Task<T?> GetDataAsync<T>(string key, CancellationToken cancellationToken = default);

    Task SetDataAsync<T>(string key, T value, DateTimeOffset expirationTime,
        CancellationToken cancellationToken = default);

    Task RemoveDataAsync(string key, CancellationToken cancellationToken = default);
}
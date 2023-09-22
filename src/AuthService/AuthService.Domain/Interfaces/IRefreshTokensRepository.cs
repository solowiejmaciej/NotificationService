using AuthService.Domain.Entities;

namespace AuthService.Domain.Interfaces;

public interface IRefreshTokensRepository
{
    Task<RefreshToken?> GetByJitAsync(string jit, CancellationToken cancellationToken = default);
    Task<RefreshToken?> GetByValueAsync(string value, CancellationToken cancellationToken = default);
    Task SetUsedAsync(RefreshToken value, CancellationToken cancellationToken = default);
    Task SaveAsync(CancellationToken cancellationToken = default);
    Task Save();
    Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
}
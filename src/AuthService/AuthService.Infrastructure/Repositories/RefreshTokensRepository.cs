using System.Threading;
using System.Threading.Tasks;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Repositories;

public class RefreshTokensRepository : IRefreshTokensRepository
{
    private readonly AuthDbContext _dbContext;

    public RefreshTokensRepository(
        AuthDbContext dbContext
        )
    {
        _dbContext = dbContext;
    }
    public async Task<RefreshToken?> GetByJitAsync(string jit, CancellationToken cancellationToken = default)
    {
        return await _dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.JwtId == jit, cancellationToken);
    }

    public async Task<RefreshToken?> GetByValueAsync(string value, CancellationToken cancellationToken = default)
    {
        return await _dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == value, cancellationToken);
    }

    public async Task SetUsedAsync(RefreshToken value, CancellationToken cancellationToken = default)
    {
        value.Used = true;
        _dbContext.RefreshTokens.Update(value);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task Save()
    {
        _dbContext.SaveChanges();
        return Task.CompletedTask;
    }

    public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        await _dbContext.AddAsync(refreshToken, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
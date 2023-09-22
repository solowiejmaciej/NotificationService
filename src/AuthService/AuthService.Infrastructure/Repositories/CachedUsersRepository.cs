using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Services;
using Microsoft.Extensions.Logging;

namespace AuthService.Infrastructure.Repositories;

public class CachedUsersRepository : IUsersRepository
{
    private readonly IUsersRepository _decorated;
    private readonly ICacheService _cacheService;
    private readonly ILogger<CachedUsersRepository> _logger;

    public CachedUsersRepository(
        IUsersRepository decorated, 
        ICacheService cacheService, 
        ILogger<CachedUsersRepository> logger)
    {
        _decorated = decorated;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<string> AddAsyncWithDefaultRole(ApplicationUser user, CancellationToken cancellationToken = default)
    {
        var key = "users";
        await _cacheService.RemoveDataAsync(key, cancellationToken);
        _logger.LogInformation("cached key {0} removed", key);
        return await _decorated.AddAsyncWithDefaultRole(user, cancellationToken);
    }

    public async Task<ApplicationUser?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        string key = $"user-{id}";
        var expiryTime = DateTimeOffset.Now.AddMinutes(5);
        var cachedUser = await _cacheService.GetDataAsync<ApplicationUser>(key, cancellationToken);
        if (cachedUser is null)
        {
            var user = await _decorated.GetByIdAsync(id, cancellationToken);
            if (user is null)
            {
                return user;
            }

            await _cacheService.SetDataAsync(key, user ,expiryTime, cancellationToken );
            return user;
        }
        
        return cachedUser;
    }
    
    public async Task<List<ApplicationUser>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        string key = "users";
        var expiryTime = DateTimeOffset.Now.AddMinutes(5);
        var cachedUsers = await _cacheService.GetDataAsync<List<ApplicationUser>>(key, cancellationToken);
        if (cachedUsers is null)
        {
            _logger.LogInformation("Fetching from db for key {0}", key);
            var users = await _decorated.GetAllAsync(cancellationToken);
            if (users is null)
            {
                return users;
            }

            await _cacheService.SetDataAsync(key, users ,expiryTime, cancellationToken );
            return users;
        }
        _logger.LogInformation("Cache hit for key {0}", key);

        return cachedUsers;
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await _decorated.SaveAsync(cancellationToken);
    }

    public async Task<bool> CanConnectAsync(CancellationToken cancellationToken = default)
    {
        return await _decorated.CanConnectAsync(cancellationToken);
    }

    public bool IsEmailInUse(string email)
    {
        return _decorated.IsEmailInUse(email);
    }

    public Task<ApplicationUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return _decorated.GetByEmailAsync(email, cancellationToken);
    }


    public void Dispose()
    {
        _decorated.Dispose();
    }
}
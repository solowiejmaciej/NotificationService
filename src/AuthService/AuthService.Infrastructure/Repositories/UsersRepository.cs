#region

using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Persistent;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;

#endregion

namespace AuthService.Infrastructure.Repositories;

public sealed class UsersRepository : IUsersRepository
{
    private readonly AuthServiceDbContext _serviceDbContext;

    public UsersRepository(AuthServiceDbContext serviceDbContext)
    {
        _serviceDbContext = serviceDbContext;
    }

    public async Task<string> AddAsyncWithDefaultRole(ApplicationUser user,
        CancellationToken cancellationToken = default)
    {
        var userInDb = await _serviceDbContext.Users.AddAsync(user, cancellationToken);
        var roleUserId = await _serviceDbContext.Roles.FirstOrDefaultAsync(r => r.Name == "User", cancellationToken);
        await _serviceDbContext.UserRoles.AddAsync(new IdentityUserRole<string>
        {
            RoleId = roleUserId!.Id,
            UserId = userInDb.Entity.Id
        }, cancellationToken);
        await _serviceDbContext.SaveChangesAsync(cancellationToken);
        return userInDb.Entity.Id;
    }

    public async Task<List<ApplicationUser>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _serviceDbContext.Users.ToListAsync(cancellationToken);
    }

    public async Task<ApplicationUser?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _serviceDbContext.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public Task<int> SoftDeleteAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public void Save()
    {
        _serviceDbContext.SaveChanges();
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await _serviceDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> CanConnectAsync(CancellationToken cancellationToken = default)
    {
        return await _serviceDbContext.Database.CanConnectAsync(cancellationToken);
    }

    public bool IsEmailInUse(string email)
    {
        return _serviceDbContext.Users.Any(u => u.Email == email);
    }

    public Task<ApplicationUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return _serviceDbContext.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task ConfirmEmailAsync(string currentUserId, CancellationToken cancellationToken = default)
    {
        var user = await GetByIdAsync(currentUserId, cancellationToken);
        if (user is null) throw new NotFoundException("User not found");
        user.EmailConfirmed = true;
        await SaveAsync(cancellationToken);
    }

    public async Task ConfirmPhoneNumberAsync(string currentUserId, CancellationToken cancellationToken = default)
    {
        var user = await GetByIdAsync(currentUserId, cancellationToken);
        if (user is null) throw new NotFoundException("User not found");
        user.PhoneNumberConfirmed = true;
        await SaveAsync(cancellationToken);
    }


    private bool _disposed;

    private void Dispose(bool disposing)
    {
        if (!_disposed)
            if (disposing)
                _serviceDbContext.Dispose();
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
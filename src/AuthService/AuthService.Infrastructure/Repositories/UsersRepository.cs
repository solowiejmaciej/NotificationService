using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Persistent;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Repositories;

 public sealed class UsersRepository : IUsersRepository
    {
        private readonly AuthDbContext _dbContext;

        public UsersRepository(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> AddAsyncWithDefaultRole(ApplicationUser user,
            CancellationToken cancellationToken = default)
        {
            var userInDb = await _dbContext.Users.AddAsync(user, cancellationToken);
            var roleUserId = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == "User", cancellationToken);
            await _dbContext.UserRoles.AddAsync(new IdentityUserRole<string>
            {
                RoleId = roleUserId!.Id,
                UserId = userInDb.Entity.Id,
            }, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return userInDb.Entity.Id;
        }

        public async Task<List<ApplicationUser>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.ToListAsync(cancellationToken);
        }

        public async Task<ApplicationUser?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public Task<int> SoftDeleteAsync(string userId)
        {
            throw new NotImplementedException();
        }
        
        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public async Task SaveAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> CanConnectAsync(CancellationToken cancellationToken = default)
        {
           return await _dbContext.Database.CanConnectAsync(cancellationToken);
        }

        public bool IsEmailInUse(string email)
        {
            return _dbContext.Users.Any(u => u.Email == email);
        }

        public Task<ApplicationUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken: cancellationToken);
        }
        

        private bool _disposed;

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
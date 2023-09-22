using AuthService.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Persistent;

public class AuthDbContext : IdentityDbContext<ApplicationUser>
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
}
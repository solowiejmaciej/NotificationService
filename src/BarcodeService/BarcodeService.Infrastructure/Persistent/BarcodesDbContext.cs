using BarcodeService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BarcodeService.Infrastructure.Persistent;

public class BarcodesDbContext : DbContext
{
    public BarcodesDbContext(DbContextOptions<BarcodesDbContext> options) : base(options)
    {
    }
    public DbSet<Product> Products { get; set; }
    public DbSet<PotentialProduct> PotentialProducts { get; set; }
}
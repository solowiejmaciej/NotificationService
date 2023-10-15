using BarcodeService.Domain.Entities;
using BarcodeService.Domain.Interfaces;
using BarcodeService.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace BarcodeService.Infrastructure.Repositories;

public class PotentialProductsRepository : IPotentialProductsRepository
{
    private readonly BarcodesDbContext _dbContext;
    private readonly IQueryable<PotentialProduct> _products;

    public PotentialProductsRepository(
        BarcodesDbContext dbContext
    )
    {
        _dbContext = dbContext;
        _products = dbContext.PotentialProducts;
    }
    public async Task AddPotentialProductAsync(PotentialProduct potentialProduct)
    {
       await _dbContext.AddAsync(potentialProduct);
       await _dbContext.SaveChangesAsync();
    }

    public async Task<PotentialProduct?> GetPotentialProductByEanAsync(string ean)
    {
        return await _products.FirstOrDefaultAsync(x => x.Ean == ean);
    }

    public async Task<List<GroupedPotentialProduct>> GetGroupedPotentialProductsAsync()
    {
        var groupedByEanAndPriceCount = await _products
            .GroupBy(product => new { product.Ean, product.Price })
            .Select(group => new GroupedPotentialProduct
            {
                Ean = group.Key.Ean,
                Count = group.Count(),
                Id = group.First().Id,
                Name = group.First().Name
            })
            .OrderBy(product => product.Ean)
            .ToListAsync();
		
        var largestCountGroups = groupedByEanAndPriceCount
            .GroupBy(group => group.Ean)
            .Select(group => group.OrderByDescending(g => g.Count).First())
            .ToList();

        return largestCountGroups;
    }

    public async Task CleanAsync()
    {
        await _dbContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE PotentialProducts");
    }
}
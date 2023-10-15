using BarcodeService.Domain.Entities;

namespace BarcodeService.Domain.Interfaces;

public interface IProductsRepository
{
    Task<Product?> GetByEanAsync(string productEan);
    Task<Product?> GetByIdAsync(Guid productId);
    Task<Product?> GetByNameAsync(string productName);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
}
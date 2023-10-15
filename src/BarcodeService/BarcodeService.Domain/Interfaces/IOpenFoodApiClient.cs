using BarcodeService.Domain.Entities;

namespace BarcodeService.Domain.Interfaces;

public interface IOpenFoodApiClient
{
    public Task<Product?> GetProductByEanAsync(string productEan);
}
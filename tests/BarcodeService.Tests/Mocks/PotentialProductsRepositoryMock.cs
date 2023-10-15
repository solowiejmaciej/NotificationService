using BarcodeService.Domain.Entities;
using BarcodeService.Domain.Interfaces;
using Moq;

namespace BarcodeService.Tests.Mocks;

internal static class PotentialProductsRepositoryMock
{
    public static Mock<IPotentialProductsRepository> GetMock()
    {
        var mock = new Mock<IPotentialProductsRepository>();

        var potentialProducts = new List<PotentialProduct>()
        {
            new PotentialProduct()
            {
                Ean = "1234567890123",
                Name = "Test product",
                Price = 1.32, 
            },
            new PotentialProduct()
            {
                Ean = "1234567890123",
                Name = "Test product 2",
                Price = 5.24, 
            },
            new PotentialProduct()
            {
                Ean = "1234567890123",
                Name = "Test product 2",
                Price = 5.24, 
            },
            new PotentialProduct()
            {
                Ean = "1234567890123",
                Name = "Test product 2",
                Price = 5.24, 
            }
        };
        
        
        mock.Setup(m => m.GetGroupedPotentialProductsAsync())
            .ReturnsAsync(potentialProducts.GroupBy(product => new { product.Ean, product.Price })
                .Select(group => new GroupedPotentialProduct
                {
                    Ean = group.Key.Ean,
                    Count = group.Count(),
                    Id = group.First().Id,
                    Name = group.First().Name
                })
                .OrderBy(product => product.Ean)
                .ToList());
        
        mock.Setup(r => r.AddPotentialProductAsync(potentialProducts[0]))
            .Returns(Task.CompletedTask);
        
        return mock;
    }
}
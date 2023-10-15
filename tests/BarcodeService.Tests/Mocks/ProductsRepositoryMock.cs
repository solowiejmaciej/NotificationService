using BarcodeService.Domain.Entities;
using BarcodeService.Domain.Interfaces;
using Moq;

namespace BarcodeService.Tests.Mocks;

public static class ProductsRepositoryMock
{
    public static Mock<IProductsRepository> GetMock()
    {
        var mock = new Mock<IProductsRepository>();

        var products = new List<Product>
        {
            new Product()
            {
                CreatedAt = DateTime.Now,
                Ean = "1234567890123",
                Name = "Test product",
                Price = 1.30,
                UpdatedAt = DateTime.Now + TimeSpan.FromDays(1)
            },
            new Product()
            {
                CreatedAt = DateTime.Now,
                Ean = "1234567890123",
                Name = "Test product2",
                Price = 5.30,
                UpdatedAt = DateTime.Now + TimeSpan.FromDays(1)
            },
            new Product()
            {
                CreatedAt = DateTime.Now,
                Ean = "1234567890123",
                Id = new Guid("C38014ED-812D-4356-941E-0827CA492FCF"),
                Name = "Test product3",
                Price = 2.50,
                UpdatedAt = DateTime.Now + TimeSpan.FromDays(1)
            },
        };
        
        mock.Setup(r => r.AddAsync(It.IsAny<Product>()))
            .Returns(Task.CompletedTask);
        ;
        mock.Setup(r => r.GetByEanAsync(It.IsAny<String>()))
            .ReturnsAsync((String ean) => products.FirstOrDefault(o => o.Ean == ean));

        mock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => products.FirstOrDefault(o => o.Id == id));
        
        mock.Setup(r => r.GetByNameAsync(It.IsAny<String>()))
            .ReturnsAsync((String name) => products.FirstOrDefault(o => o.Name == name));
        
        return mock;
    }
}


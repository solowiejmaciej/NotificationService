using BarcodeService.Domain.Entities;
using BarcodeService.Domain.Interfaces;
using MediatR;

namespace BarcodeService.Application.MediatR.Command;

public class AddNewPotentialProductCommandHandler :IRequestHandler<AddNewPotentialProductCommand>
{
    private readonly IPotentialProductsRepository _repository;

    public AddNewPotentialProductCommandHandler(
        IPotentialProductsRepository repository
        )
    {
        _repository = repository;
    }
    public async Task Handle(AddNewPotentialProductCommand request, CancellationToken cancellationToken)
    {
        var product = new PotentialProduct
        {
            Price = request.Price,
            Name = request.Name,
            Ean = request.Ean
        };
        product.Ean = request.Ean;
        await _repository.AddPotentialProductAsync(product);
    }
}

public record AddNewPotentialProductCommand : IRequest
{
    public required string Ean { get; set; }
    public required double Price { get; set; }
    public required string Name { get; set; }
}
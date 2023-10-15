using AutoMapper;
using BarcodeService.Application.Dtos;
using BarcodeService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;

namespace BarcodeService.Application.MediatR.Query;


public class GetProductByEanQueryHandler : IRequestHandler<GetProductByEanQuery, ProductDto?>
{
    private readonly IProductsRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProductByEanQueryHandler> _logger;


    public GetProductByEanQueryHandler(
        IProductsRepository productRepository, 
        IMapper mapper,
        ILogger<GetProductByEanQueryHandler> logger
        )
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;

    }

    public async Task<ProductDto?> Handle(GetProductByEanQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByEanAsync(request.Ean);
        if (product == null)
        {
            throw new NotFoundException($"Product with EAN {request.Ean} not found");
        }
        return _mapper.Map<ProductDto>(product);
    }
}
public record GetProductByEanQuery :IRequest<ProductDto?>
{
    public required string Ean { get; set; }
}


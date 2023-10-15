using AutoMapper;
using BarcodeService.Domain.Entities;
using BarcodeService.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace BarcodeService.Application.Services;

public class PotentialProductsEvaluationService : IPotentialProductsEvaluationService
{
    private readonly ILogger<PotentialProductsEvaluationService> _logger;
    private readonly IPotentialProductsRepository _potentialProductsRepository;
    private readonly IProductsRepository _productsRepository;
    private readonly IMapper _mapper;

    public PotentialProductsEvaluationService(
        ILogger<PotentialProductsEvaluationService> logger,
        IPotentialProductsRepository potentialProductsRepository,
        IProductsRepository productsRepository,
        IMapper mapper
        )
    {
        _logger = logger;
        _potentialProductsRepository = potentialProductsRepository;
        _productsRepository = productsRepository;
        _mapper = mapper;
    }
    public async Task Eval()
    {
        var result = await _potentialProductsRepository.GetGroupedPotentialProductsAsync();
        
        if (result.Count == 0)
        {
            _logger.LogInformation("No potential products to add");
            return;
        }
        _logger.LogInformation($"Got {result.Count} potential products to add");

         foreach (var potentialProduct in result)
        {
            var product = _mapper.Map<Product>(potentialProduct);
            _logger.LogInformation($"Adding product {product.Ean}");
            await _productsRepository.AddAsync(product);
        }
        _logger.LogInformation("Finished adding products");
        _logger.LogInformation("Going to clean potential products table");
        await _potentialProductsRepository.CleanAsync();
    }
}
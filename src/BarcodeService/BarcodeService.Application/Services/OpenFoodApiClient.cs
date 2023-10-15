using AutoMapper;
using BarcodeService.Application.Models;
using BarcodeService.Application.Models.AppSettings;
using BarcodeService.Domain.Entities;
using BarcodeService.Domain.Interfaces;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BarcodeService.Application.Services;

public class OpenFoodApiClient : IOpenFoodApiClient
{
    private readonly IMapper _mapper;
    private readonly OpenFoodApiSettings _openApiSettings;
    private readonly ILogger<OpenFoodApiClient> _logger;

    public OpenFoodApiClient(
        IMapper mapper,
        IOptions<OpenFoodApiSettings> openApiSettings,
        ILogger<OpenFoodApiClient> logger
        )
    {
        _mapper = mapper;
        _openApiSettings = openApiSettings.Value;
        _logger = logger;
    }
    public async Task<Product?> GetProductByEanAsync(string productEan)
    {
        try
        {
            var baseUrl = "https://openfoodfacts.org/api/v3/";
            var result = await $"{baseUrl}product/{productEan}"
                .SetQueryParams(new
                {
                    fields = "product_name,brands"
                })
                .GetAsync()
                .ReceiveJson<OpenApiResponse>();
            var product = _mapper.Map<Product>(result);
            return product;
        }
        catch (FlurlHttpException ex)
        {
            _logger.LogError("Error while getting product from OpenFoodApi: {@ex}", ex);
            return null;
        }
    }
}
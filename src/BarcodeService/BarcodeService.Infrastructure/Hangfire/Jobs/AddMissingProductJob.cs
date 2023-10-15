using BarcodeService.Domain.Entities;
using BarcodeService.Domain.Interfaces;
using Hangfire;
using Hangfire.Server;
using Microsoft.Extensions.Logging;

namespace BarcodeService.Application.Hangfire.Jobs;

public sealed class AddMissingProductJob
{
    private readonly IProductsRepository _repository;
    private readonly ILogger<AddMissingProductJob> _logger;

    public AddMissingProductJob(
        IProductsRepository repository,
        ILogger<AddMissingProductJob> logger
        )
    {
        _repository = repository;
        _logger = logger;
    }
    [AutomaticRetry(Attempts = HangfireRetryAttempts.DEFAULT)]
    [JobDisplayName("AddMissingProductJob")]
    [Queue(HangfireQueues.DEFAULT)]
    public async Task Invoke(
        Product product,
        PerformContext context,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("AddMissingProductJob invoked for {Ean}", product.Ean);
        await _repository.AddAsync(product);
    }
}
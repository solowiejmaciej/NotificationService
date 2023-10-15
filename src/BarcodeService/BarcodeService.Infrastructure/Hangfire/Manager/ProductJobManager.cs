using BarcodeService.Application.Hangfire.Jobs;
using BarcodeService.Domain.Entities;
using BarcodeService.Domain.Interfaces;
using Hangfire;

namespace BarcodeService.Infrastructure.Hangfire.Manager;

public class ProductJobManager : IProductJobManager
{
    private readonly IBackgroundJobClient _backgroundJobClient;

    public ProductJobManager(IBackgroundJobClient backgroundJobClient)
    {
        _backgroundJobClient = backgroundJobClient;
    }
    public void EnqueueAddMissingProductJob(Product product)
    {
        _backgroundJobClient.Enqueue<AddMissingProductJob>(x =>
            x.Invoke(
                product,
                default!,
                CancellationToken.None));
    }
}
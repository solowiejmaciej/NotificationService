using BarcodeService.Domain.Entities;

namespace BarcodeService.Domain.Interfaces;

public interface IProductJobManager
{
    public void EnqueueAddMissingProductJob(Product product);
}
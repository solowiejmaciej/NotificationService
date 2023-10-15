using BarcodeService.Domain.Entities;

namespace BarcodeService.Domain.Interfaces;

public interface IPotentialProductsRepository
{
    Task AddPotentialProductAsync(PotentialProduct potentialProduct);
    Task<List<GroupedPotentialProduct>> GetGroupedPotentialProductsAsync();
    Task CleanAsync();
}
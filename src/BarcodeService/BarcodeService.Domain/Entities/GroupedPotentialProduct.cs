namespace BarcodeService.Domain.Entities;

public class GroupedPotentialProduct
{
    public Guid Id { get; set; }
    public string Ean { get; set; }
    public decimal Price { get; set; }
    public string Name { get; set; }
    public int Count { get; set; }
}
namespace BarcodeService.Domain.Entities;

public class PotentialProduct
{
    public Guid Id { get; set; }
    public double Price { get; set; }
    public string Ean { get; set; }
    public string Name { get; set; }
}
namespace BarcodeService.Api.Models.Requests;

public class AddNewPotentialProductRequest
{
    public string Ean { get; set; }
    public double Price { get; set; }
    public string Name { get; set; }
}
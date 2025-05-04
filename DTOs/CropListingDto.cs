using CropDeals.Models;

public class CropListingCreateDto
{
    public Guid CropId { get; set; }
    public float PricePerKg { get; set; }
    public int Quantity { get; set; }
    public CropAvailability Status { get; set; }
    public string? ImageBase64 { get; set; }
    public string? Description { get; set; }
}

public class CropListingUpdateDto
{
    public Guid Id { get; set; }
    public float PricePerKg { get; set; }
    public int Quantity { get; set; }
    public CropAvailability Status { get; set; }
    public string? ImageBase64 { get; set; }
    public string? Description { get; set; }
}

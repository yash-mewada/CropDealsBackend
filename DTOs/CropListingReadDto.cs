using CropDeals.Models;

public class CropListingReadDto
{
    public Guid Id { get; set; }
    public Guid CropId { get; set; }
    public string CropName { get; set; }
    public Guid FarmerId { get; set; }
    public string FarmerName { get; set; }
    public float PricePerKg { get; set; }
    public int Quantity { get; set; }
    public CropAvailability Status { get; set; }
    public string? ImageBase64 { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}

using CropDeals.Models;

public class ListingNotificationDTO
{
    public Guid ListingId { get; set; }
    public string CropName { get; set; }
    public string FarmerName { get; set; }
    public decimal PricePerKg { get; set; }
    public int Quantity { get; set; }
    public string Description { get; set; }
    public CropAvailability Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

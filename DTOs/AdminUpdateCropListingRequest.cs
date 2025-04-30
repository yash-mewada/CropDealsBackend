public class AdminUpdateCropListingRequest
{
    public decimal PricePerKg { get; set; }
    public int Quantity { get; set; }
    public string Status { get; set; } = "Available"; // Optional: Enum conversion
    public string ImageUrl { get; set; }
    public string Description { get; set; }
}

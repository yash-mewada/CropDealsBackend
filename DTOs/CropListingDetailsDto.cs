public class CropListingDetailsDto
{
    public string ListingId { get; set; }
    public string CropName { get; set; }
    public string? Description { get; set; }
    public string? ImageBase64 { get; set; }

    public float PricePerKg { get; set; }
    public int Quantity { get; set; }

    // Farmer Info
    public string FarmerName { get; set; }
    public string FarmerPhoneNumber { get; set; }

    // Location
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
}

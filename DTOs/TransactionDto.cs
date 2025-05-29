// Models/DTOs/TransactionDTO.cs
using CropDeals.Models;

namespace CropDeals.DTOs
{
    public class TransactionDTO
    {
        public Guid Id { get; set; }
        public Guid DealerId { get; set; }
        public string DealerName { get; set; }          // ✅ New
        public Guid ListingId { get; set; }
        public string CropName { get; set; }
        public string FarmerName { get; set; }
        public string Description { get; set; }
        public string ImageBase64 { get; set; }
        public int Quantity { get; set; }
        public float FinalPricePerKg { get; set; }
        public float TotalPrice { get; set; }
        public TransactionStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

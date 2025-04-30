using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CropDeals.Models
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid DealerId { get; set; }

        [ForeignKey(nameof(DealerId))]
        public ApplicationUser Dealer { get; set; }

        [Required]
        public Guid ListingId { get; set; }

        [ForeignKey(nameof(ListingId))]
        public CropListing Listing { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public float FinalPricePerKg { get; set; }

        [Required]
        public float TotalPrice { get; set; }

        [Required]
        public TransactionStatus Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum TransactionStatus
    {
        Pending,
        Completed,
        Cancelled
    }
}

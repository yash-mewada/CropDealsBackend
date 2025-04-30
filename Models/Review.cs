using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CropDeals.Models
{
    public class Review
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid DealerId { get; set; }

        [ForeignKey(nameof(DealerId))]
        public ApplicationUser Dealer { get; set; }

        [Required]
        public Guid FarmerId { get; set; }

        [ForeignKey(nameof(FarmerId))]
        public ApplicationUser Farmer { get; set; }

        [Required]
        public Guid TransactionId { get; set; }

        [ForeignKey(nameof(TransactionId))]
        public Transaction Transaction { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

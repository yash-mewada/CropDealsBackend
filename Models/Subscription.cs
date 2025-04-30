using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CropDeals.Models
{
    public class Subscription
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid DealerId { get; set; }

        [ForeignKey(nameof(DealerId))]
        public ApplicationUser Dealer { get; set; }

        [Required]
        public Guid CropId { get; set; }

        [ForeignKey(nameof(CropId))]
        public Crop Crop { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

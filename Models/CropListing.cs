using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CropDeals.Models
{
    public class CropListing
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid FarmerId { get; set; }

        [ForeignKey(nameof(FarmerId))]
        public ApplicationUser Farmer { get; set; }

        [Required]
        public Guid CropId { get; set; }

        [ForeignKey(nameof(CropId))]
        public Crop Crop { get; set; }

        [Required]
        public float PricePerKg { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public CropAvailability Status { get; set; }

        public string? ImageUrl { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum CropAvailability
    {
        Available,
        OutOfStock
    }
}

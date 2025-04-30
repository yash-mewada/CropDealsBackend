using System.ComponentModel.DataAnnotations;

namespace CropDeals.Models
{
    public class Crop
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public CropTypeEnum Type { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum CropTypeEnum
    {
        Fruit,
        Vegetable,
        Grain
    }
}

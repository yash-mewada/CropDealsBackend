using System.ComponentModel.DataAnnotations;

namespace CropDeals.Models.DTOs
{
    public class SubscriptionCreateDto
    {
        [Required]
        public Guid CropId { get; set; }
    }
}

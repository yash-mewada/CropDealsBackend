namespace CropDeals.Models.DTOs
{
    public class SubscriptionReadDto
    {
        public Guid Id { get; set; }
        public Guid CropId { get; set; }
        public string CropName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

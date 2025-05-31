namespace CropDeals.DTOs
{
    public class AddReviewRequest
    {
        public Guid TransactionId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }

    public class ReviewDTO
    {
        public Guid Id { get; set; }
        public Guid DealerId { get; set; }
        public string DealerName { get; set; }
        public Guid FarmerId { get; set; }
        public string FarmerName { get; set; }
        public Guid TransactionId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

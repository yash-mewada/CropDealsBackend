namespace CropDeals.DTOs
{
    public class AddReviewRequest
    {
        public Guid TransactionId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}

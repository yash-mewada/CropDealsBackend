namespace CropDeals.DTOs
{
    public class GenerateReportRequest
    {
        public string Title { get; set; }
        public string? Content { get; set; }
        public Guid GeneratedFor { get; set; }
    }
}

public class CreateTransactionRequest
{
    public Guid ListingId { get; set; }
    public int Quantity { get; set; }
    public float FinalPricePerKg { get; set; }
}

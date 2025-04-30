public interface ITransactionRepository
{
    Task<string> CreateTransactionAsync(Guid dealerId, CreateTransactionRequest request);
}

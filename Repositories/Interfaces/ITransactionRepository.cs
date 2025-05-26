using CropDeals.Models;
public interface ITransactionRepository
{
    Task<Transaction?> CreateTransactionAsync(Guid dealerId, CreateTransactionRequest request);
}

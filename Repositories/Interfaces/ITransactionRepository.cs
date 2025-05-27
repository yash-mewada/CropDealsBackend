using CropDeals.Models;
public interface ITransactionRepository
{
    Task<Transaction?> CreateTransactionAsync(Guid dealerId, CreateTransactionRequest request);
    Task<List<Transaction>> GetTransactionsByDealerIdAsync(Guid dealerId);

}

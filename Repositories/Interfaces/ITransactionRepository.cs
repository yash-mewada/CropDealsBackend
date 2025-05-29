using CropDeals.DTOs;
using CropDeals.Models;
public interface ITransactionRepository
{
    Task<Transaction?> CreateTransactionAsync(Guid dealerId, CreateTransactionRequest request);
    Task<List<TransactionDTO>> GetTransactionsByDealerIdAsync(Guid dealerId);


}

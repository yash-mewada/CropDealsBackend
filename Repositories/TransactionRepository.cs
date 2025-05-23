using CropDeals.Data;
using CropDeals.Models;
using CropDeals.Models.DTOs;
using Microsoft.EntityFrameworkCore;

public class TransactionRepository : ITransactionRepository
{
    private readonly ApplicationDbContext _context;

    public TransactionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> CreateTransactionAsync(Guid dealerId, CreateTransactionRequest request)
    {
        var listing = await _context.CropListings.FindAsync(request.ListingId);
        if (listing == null) return "Crop listing not found.";

        if (listing.Quantity < request.Quantity)
            return "Requested quantity exceeds available stock.";

        // Calculate total
        float totalPrice = request.FinalPricePerKg * request.Quantity;

        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            DealerId = dealerId,
            ListingId = listing.Id,
            Quantity = request.Quantity,
            FinalPricePerKg = request.FinalPricePerKg,
            TotalPrice = totalPrice,
            Status = TransactionStatus.Completed,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        listing.Quantity -= request.Quantity;
        if (listing.Quantity == 0)
        {
            listing.Status = CropAvailability.OutOfStock;
        }

        _context.Transactions.Add(transaction);
        _context.CropListings.Update(listing);
        await _context.SaveChangesAsync();

        return "Transaction completed successfully.";
    }
}

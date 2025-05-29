using CropDeals.Data;
using CropDeals.DTOs;
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

    public async Task<Transaction?> CreateTransactionAsync(Guid dealerId, CreateTransactionRequest request)
    {
        var listing = await _context.CropListings.FindAsync(request.ListingId);
        if (listing == null) return null;

        if (listing.Quantity < request.Quantity)
            return null;

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

        return transaction;
    }

    public async Task<List<TransactionDTO>> GetTransactionsByDealerIdAsync(Guid dealerId)
    {
        return await _context.Transactions
            .Where(t => t.DealerId == dealerId)
            .Include(t => t.Dealer)
            .Include(t => t.Listing)
                .ThenInclude(l => l.Crop)
            .Include(t => t.Listing)
                .ThenInclude(l => l.Farmer)
            .Select(t => new TransactionDTO
            {
                Id = t.Id,
                DealerId = t.DealerId,
                DealerName = t.Dealer.Name,
                ListingId = t.ListingId,
                CropName = t.Listing.Crop.Name,
                FarmerName = t.Listing.Farmer.Name,
                Description = t.Listing.Description,
                ImageBase64 = t.Listing.ImageBase64,
                Quantity = t.Quantity,
                FinalPricePerKg = t.FinalPricePerKg,
                TotalPrice = t.TotalPrice,
                Status = t.Status,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            })
            .ToListAsync();
    }
}

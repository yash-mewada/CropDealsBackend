using CropDeals.Data;
using CropDeals.DTOs;
using CropDeals.Models;
using CropDeals.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CropDeals.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddReviewAsync(string dealerId, AddReviewRequest request)
        {
            var transaction = await _context.Transactions
                .Include(t => t.Listing)
                .FirstOrDefaultAsync(t => t.Id == request.TransactionId);

            if (transaction == null)
                return "Transaction not found.";

            if (transaction.DealerId.ToString() != dealerId)
                return "Unauthorized to review this transaction.";

            var farmerId = transaction.Listing.FarmerId;

            var review = new Review
            {
                Id = Guid.NewGuid(),
                DealerId = Guid.Parse(dealerId),
                FarmerId = farmerId,
                TransactionId = transaction.Id,
                Rating = request.Rating,
                Comment = request.Comment,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return "Review added successfully.";
        }
    }
}

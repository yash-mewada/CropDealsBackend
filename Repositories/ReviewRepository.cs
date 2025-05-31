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

        public async Task<List<ReviewDTO>> GetMyReviewsAsync(string userId, string role)
        {
            Guid userGuid = Guid.Parse(userId);

            var query = _context.Reviews
                .Include(r => r.Dealer)
                .Include(r => r.Farmer)
                .AsQueryable();

            if (role == "Dealer")
                query = query.Where(r => r.DealerId == userGuid);
            else if (role == "Farmer")
                query = query.Where(r => r.FarmerId == userGuid);
            else
                return new List<ReviewDTO>(); // Unauthorized roles

            return await query
                .Select(r => new ReviewDTO
                {
                    Id = r.Id,
                    DealerId = r.DealerId,
                    DealerName = r.Dealer.Name,
                    FarmerId = r.FarmerId,
                    FarmerName = r.Farmer.Name,
                    TransactionId = r.TransactionId,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();
        }

    }
}

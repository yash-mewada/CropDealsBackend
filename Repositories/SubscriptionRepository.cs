using CropDeals.Data;
using CropDeals.Models;
using CropDeals.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CropDeals.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Subscription> AddSubscriptionAsync(Guid dealerId, Subscription subscription)
        {
            subscription.DealerId = dealerId;
            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();
            return subscription;
        }

        public async Task<IEnumerable<Subscription>> GetDealerSubscriptionsAsync(Guid dealerId)
        {
            return await _context.Subscriptions
                .Include(s => s.Crop)
                .Where(s => s.DealerId == dealerId)
                .ToListAsync();
        }

        public async Task<bool> UnsubscribeAsync(Guid id, Guid dealerId)
        {
            var sub = await _context.Subscriptions.FirstOrDefaultAsync(s => s.Id == id && s.DealerId == dealerId);
            if (sub == null) return false;

            _context.Subscriptions.Remove(sub);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ListingNotificationDTO>> GetNotificationsAsync(Guid dealerId)
        {
            var subscribedCropIds = await _context.Subscriptions
                .Where(s => s.DealerId == dealerId)
                .Select(s => s.CropId)
                .ToListAsync();

            var listings = await _context.CropListings
                .Include(l => l.Crop)
                .Include(l => l.Farmer)
                .Where(l => subscribedCropIds.Contains(l.CropId) && l.Status == 0)
                .OrderByDescending(l => l.CreatedAt)
                .Select(l => new ListingNotificationDTO
                {
                    ListingId = l.Id,
                    CropName = l.Crop.Name,
                    FarmerName = l.Farmer.Name,
                    PricePerKg = (decimal)l.PricePerKg,
                    Quantity = l.Quantity,
                    Description = l.Description,
                    CreatedAt = l.CreatedAt,
                    Status = l.Status
                })
                .ToListAsync();

            return listings;
        }
    }
}

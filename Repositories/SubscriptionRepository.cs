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
    }
}

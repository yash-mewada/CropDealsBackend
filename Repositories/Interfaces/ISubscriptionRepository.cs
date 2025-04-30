using CropDeals.Models;
using System.Security.Claims;

namespace CropDeals.Repositories.Interfaces
{
    public interface ISubscriptionRepository
    {
        Task<Subscription> AddSubscriptionAsync(Guid dealerId, Subscription subscription);
        Task<IEnumerable<Subscription>> GetDealerSubscriptionsAsync(Guid dealerId);
        Task<bool> UnsubscribeAsync(Guid id, Guid dealerId);
    }
}

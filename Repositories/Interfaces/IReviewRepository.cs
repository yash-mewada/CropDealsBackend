using CropDeals.Models;
using CropDeals.DTOs;

namespace CropDeals.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        Task<string> AddReviewAsync(string dealerId, AddReviewRequest request);
        Task<List<ReviewDTO>> GetMyReviewsAsync(string userId, string role);

    }
}

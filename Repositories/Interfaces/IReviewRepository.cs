using CropDeals.Models;
using CropDeals.DTOs;

namespace CropDeals.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        Task<string> AddReviewAsync(string dealerId, AddReviewRequest request);
    }
}

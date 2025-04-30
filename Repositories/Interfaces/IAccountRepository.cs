using CropDeals.DTOs;
using CropDeals.Models;
using CropDeals.Models.DTOs;
using System.Threading.Tasks;

namespace CropDeals.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<string> RegisterAsync(SignUpRequest request);
        Task<string> UpdateProfileAsync(string userId, UpdateProfileRequest request);
        Task<string> AdminEditUserAsync(string targetUserId, AdminEditUserProfileRequest request);
        Task<string> AdminDeleteUserAsync(string targetUserId);
    }
}

using CropDeals.Models;
using CropDeals.DTOs;

namespace CropDeals.Repositories.Interfaces
{
    public interface IReportRepository
    {
        Task<string> GenerateReportAsync(string adminId, GenerateReportRequest request);
    }
}

using CropDeals.Data;
using CropDeals.DTOs;
using CropDeals.Models;
using CropDeals.Repositories.Interfaces;

namespace CropDeals.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext _context;

        public ReportRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateReportAsync(string adminId, GenerateReportRequest request)
        {
            var report = new Report
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Content = request.Content,
                GeneratedBy = Guid.Parse(adminId),
                GeneratedFor = request.GeneratedFor,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            return "Report generated successfully.";
        }
    }
}
